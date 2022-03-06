namespace VendingMachine.Domain.Commands.Handlers
{
    using MediatR;
    using Microsoft.Extensions.Logging;
    using VendingMachine.Domain.Events;
    using VendingMachine.Domain.Models;
    using VendingMachine.Domain.Services;

    public class ReturnUserCoinsHandler : IRequestHandler<ReturnUserCoinsCommand, IEnumerable<CoinWithQuantityDto>>
    {
        private readonly ILogger<InsertCoinsHandler> _logger;
        private readonly IWalletService _walletService;
        private readonly IMediator _mediator;

        public ReturnUserCoinsHandler(ILogger<InsertCoinsHandler> logger, IWalletService walletService, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<CoinWithQuantityDto>> Handle(ReturnUserCoinsCommand request, CancellationToken cancellationToken)
        {
            var coinsToReturn = request.Coins;
            if (!coinsToReturn.Any())
            {
                coinsToReturn = await _walletService.GetCustomerWalletAsync() ?? Enumerable.Empty<CoinWithQuantityDto>();
                await _walletService.ClearCustomerWalletAsync();
            }

            await _mediator.Publish(new CreditReturnEvent(coinsToReturn));
            _logger.LogInformation($"Coins returned to user: { string.Join(", ", coinsToReturn.Select(x => $"[{x.CoinValue}: {x.Quantity}]")) }");

            return coinsToReturn;
        }
    }
}
