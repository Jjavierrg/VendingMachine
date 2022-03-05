namespace VendingMachine.Core.Commands
{
    using MediatR;
    using VendingMachine.Core.Models;

    public class InsertCoinsCommand : IRequest<UserCreditDto>
    {
        public InsertCoinsCommand(params CoinWithQuantityDto[] coins)
        {
            Coins = coins;
        }

        public IEnumerable<CoinWithQuantityDto> Coins { get; }
    }
}
