namespace VendingMachine.Core.Commands.Validators
{
    using FluentValidation;
    using System;
    using VendingMachine.Entities;
    using VendingMachine.Infrastructure.Core;

    public class InsertCoinsCommandValidator : AbstractValidator<InsertCoinsCommand>
    {
        public InsertCoinsCommandValidator(IReadRepository<Coin> coinRepository)
        {
            if (coinRepository is null)
                throw new ArgumentNullException(nameof(coinRepository));

            RuleFor(x => x.Coins).NotNull().NotEmpty();
            RuleSet("ValidCoins", () => {
                RuleFor(x => x.Coins).MustAsync(async (coins, cancellation) =>
                {
                    var validCoins = await coinRepository.GetListAsync();
                    return coins.All(c => validCoins.Any(x => x.Value == c.CoinValue));
                }).WithMessage("Invalid Coins");
            });
                
        }
    }
}
