namespace VendingMachine.Domain.Commands
{
    using MediatR;
    using VendingMachine.Domain.Models;

    public class ReturnUserCoinsCommand : IRequest<IEnumerable<CoinWithQuantityDto>>
    {
    }
}
