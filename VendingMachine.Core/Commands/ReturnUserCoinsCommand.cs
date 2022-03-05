namespace VendingMachine.Core.Commands
{
    using MediatR;
    using VendingMachine.Core.Models;

    public class ReturnUserCoinsCommand : IRequest<IEnumerable<CoinWithQuantityDto>>
    {
    }
}
