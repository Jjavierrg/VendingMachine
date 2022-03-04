namespace VendingMachine.Core.Commands
{
    using MediatR;
    using VendingMachine.Core.Models;

    public class InsertCoinsCommand : IRequest<UserCreditDto>
    {
        public InsertCoinsCommand(params WalletCoinDto[] coins)
        {
            Coins = coins;
        }

        public IEnumerable<WalletCoinDto> Coins { get; }
    }
}
