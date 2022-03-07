namespace VendingMachine.Domain.Commands
{
    using MediatR;
    using VendingMachine.Domain.Models;

    public class ReturnUserCoinsCommand : IRequest<IEnumerable<CoinWithQuantityDto>>
    {
        public ReturnUserCoinsCommand(): this(Enumerable.Empty<CoinWithQuantityDto>())
        {
        }

        public ReturnUserCoinsCommand(IEnumerable<CoinWithQuantityDto> coins)
        {
            Coins = coins ?? Enumerable.Empty<CoinWithQuantityDto>();
        }

        public IEnumerable<CoinWithQuantityDto> Coins { get; }
    }
}
