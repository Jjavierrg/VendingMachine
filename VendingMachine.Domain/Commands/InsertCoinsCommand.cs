namespace VendingMachine.Domain.Commands
{
    using MediatR;
    using VendingMachine.Domain.Models;

    public class InsertCoinsCommand : IRequest<UserCreditDto>
    {
        public InsertCoinsCommand(params CoinWithQuantityDto[] coins)
        {
            Coins = coins;
        }

        public IEnumerable<CoinWithQuantityDto> Coins { get; }
    }
}
