namespace VendingMachine.Domain.Commands.Handlers
{
    using MediatR;
    using Microsoft.Extensions.Logging;
    using VendingMachine.Domain.Exceptions;
    using VendingMachine.Domain.Models;
    using VendingMachine.Domain.Services;

    public class SellProductHandler : IRequestHandler<SellProductCommand, SellDto>
    {
        private readonly ILogger<SellProductHandler> _logger;
        private readonly ISellService _sellService;
        private readonly IWalletService _walletService;
        private readonly IChangeService _changeService;

        public SellProductHandler(ILogger<SellProductHandler> logger, ISellService sellService, IWalletService walletService, IChangeService changeService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sellService = sellService ?? throw new ArgumentNullException(nameof(sellService));
            _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));
            _changeService = changeService ?? throw new ArgumentNullException(nameof(changeService));
        }

        public async Task<SellDto> Handle(SellProductCommand request, CancellationToken cancellationToken)
        {
            var hasStock = await _sellService.SlotHasEnoughStock(request.SlotNumber, request.Quantity);
            if (!hasStock)
            {
                _logger.LogError($"No enough stock. Slot ordered: {request.SlotNumber} | Quantity: {request.Quantity}");
                throw new OutOfStockException();
            }

            var orderPrice = await _sellService.GetOrderPrice(request.SlotNumber, request.Quantity);
            var userCredit = await _walletService.GetCustomerCreditAsync();
            if (orderPrice > userCredit.Credit)
            {
                _logger.LogError($"No enough credit. Order Price: {orderPrice} | User Credit: {userCredit}");
                throw new InsufficientCreditException(orderPrice - userCredit.Credit);
            }

            IEnumerable<CoinWithQuantityDto>? coinsToReturn;
            try
            {
                var changeToReturn = userCredit.Credit - orderPrice;
                coinsToReturn = await _changeService.GetCoinChangeAsync(changeToReturn);

                await _walletService.MoveCustomerWalletToMachineWalletAsync();
                await _walletService.GetCoinsFromMachineWalletAsync(coinsToReturn);
            }
            catch (InsufficientChangeException)
            {
                _logger.LogError($"No enough change");
                throw;
            }

            var slot = await _sellService.DiscountQuantityAndGetNewStock(request.SlotNumber, request.Quantity);
            return new SellDto
            {
                Quantity = request.Quantity,
                OrderPrice = orderPrice,
                Product = slot,
                ChangeCoins = coinsToReturn ?? Enumerable.Empty<CoinWithQuantityDto>()
            };
        }
    }
}
