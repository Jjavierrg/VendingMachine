namespace VendingMachine.Domain.Commands.Handlers
{
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using VendingMachine.Domain.Events;
    using VendingMachine.Domain.Exceptions;
    using VendingMachine.Domain.Models;
    using VendingMachine.Domain.Services;

    public class InsertCoinsHandler : IRequestHandler<InsertCoinsCommand, UserCreditDto>
    {
        private readonly ILogger<InsertCoinsHandler> _logger;
        private readonly IWalletService _walletService;
        private readonly IValidator<InsertCoinsCommand> _validator;
        private readonly IMediator _mediator;

        public InsertCoinsHandler(ILogger<InsertCoinsHandler> logger, IWalletService walletService, IValidator<InsertCoinsCommand> validator, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<UserCreditDto> Handle(InsertCoinsCommand request, CancellationToken cancellationToken)
        {
            var coinsAreValid = await AreAllCoinsValid(request);
            if (!coinsAreValid)
            {
                await _mediator.Publish(new ReturnUserCoinsCommand(request.Coins));
                throw new InvalidCoinsException();
            }

            var hasCoins = request?.Coins?.Any() ?? false;
            if (hasCoins)
            {
                await _walletService.AddCoinsToCustomerWalletAsync(request.Coins);
                _logger.LogInformation($"Added coins to wallet: { string.Join(", ", request.Coins.Select(x => $"[{x.CoinValue}: {x.Quantity}]")) }");
            }

            var credit = await _walletService.GetCustomerCreditAsync();
            await _mediator.Publish(new CreditAddedEvent(credit));
            _logger.LogInformation($"New User Credit: { credit.Credit }");

            return credit;
        }

        private async Task<bool> AreAllCoinsValid(InsertCoinsCommand request)
        {
            var hasCoins = request?.Coins?.Any() ?? false;
            if (!hasCoins)
                return true;

            var result = await _validator.ValidateAsync(request, options => options.IncludeRuleSets("ValidCoins"));
            return result.IsValid;
        }
    }
}
