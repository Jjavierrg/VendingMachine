namespace VendingMachine.Core.Commands.Handlers
{
    using MediatR;
    using Microsoft.Extensions.Logging;
    using VendingMachine.Core.Models;
    using VendingMachine.Core.Services;

    public class ReturnUserCoinsHandler : IRequestHandler<ReturnUserCoinsCommand, IEnumerable<CoinWithQuantityDto>>
    {
        private readonly ILogger<InsertCoinsHandler> _logger;
        private readonly IWalletService _walletService;

        public ReturnUserCoinsHandler(ILogger<InsertCoinsHandler> logger, IWalletService walletService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));
        }

        public async Task<IEnumerable<CoinWithQuantityDto>> Handle(ReturnUserCoinsCommand request, CancellationToken cancellationToken)
        {
            var userWallet = await _walletService.GetCustomerWalletAsync() ?? new List<CoinWithQuantityDto>();
            await _walletService.ClearCustomerWalletAsync();
            _logger.LogInformation($"Coins returned to user: { string.Join(", ", userWallet.Select(x => $"[{x.CoinValue}: {x.Quantity}]")) }");

            return userWallet;
        }
    }
}
