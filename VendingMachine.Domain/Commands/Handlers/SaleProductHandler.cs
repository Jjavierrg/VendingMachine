namespace VendingMachine.Domain.Commands.Handlers
{
    using MediatR;
    using Microsoft.Extensions.Logging;
    using VendingMachine.Domain.Events;
    using VendingMachine.Domain.Exceptions;
    using VendingMachine.Domain.Models;
    using VendingMachine.Domain.Services;

    public class SaleProductHandler : IRequestHandler<SaleProductCommand, SaleDto>
    {
        private readonly ILogger<SaleProductHandler> _logger;
        private readonly ISaleService _saleService;
        private readonly IWalletService _walletService;
        private readonly IChangeService _changeService;
        private readonly IMediator _mediator;

        public SaleProductHandler(ILogger<SaleProductHandler> logger, ISaleService salelService, IWalletService walletService, IChangeService changeService, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _saleService = salelService ?? throw new ArgumentNullException(nameof(salelService));
            _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));
            _changeService = changeService ?? throw new ArgumentNullException(nameof(changeService));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<SaleDto> Handle(SaleProductCommand request, CancellationToken cancellationToken)
        {
            var hasStock = await _saleService.SlotHasEnoughStock(request.SlotNumber, request.Quantity);
            if (!hasStock)
            {
                _logger.LogError($"No enough stock. Slot ordered: {request.SlotNumber} | Quantity: {request.Quantity}");
                throw new OutOfStockException();
            }

            var orderPrice = await _saleService.GetOrderPrice(request.SlotNumber, request.Quantity);
            var userCredit = await _walletService.GetCustomerCreditAsync();
            if (orderPrice > userCredit.Credit)
            {
                _logger.LogError($"No enough credit. Order Price: {orderPrice} | User Credit: {userCredit.Credit}");
                await _mediator.Publish(new InsufficientCreditEvent(orderPrice, userCredit.Credit));
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
                await _mediator.Publish(new InsufficientChangeEvent(userCredit.Credit - orderPrice));
                _logger.LogError($"No enough change");
                throw;
            }

            var slot = await _saleService.DiscountQuantityAndGetNewStock(request.SlotNumber, request.Quantity);
            var sale = new SaleDto
            {
                Quantity = request.Quantity,
                OrderPrice = orderPrice,
                Product = slot,
                ChangeCoins = coinsToReturn ?? Enumerable.Empty<CoinWithQuantityDto>()
            };

            await _mediator.Publish(new ProductSoldEvent(sale));
            return sale;
        }
    }
}
