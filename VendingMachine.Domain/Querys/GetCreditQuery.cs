namespace VendingMachine.Domain.Querys
{
    using MediatR;
    using VendingMachine.Domain.Models;

    public class GetAvailableCoinsQuery : IRequest<IEnumerable<CoinWithQuantityDto>>
    {
    }
}
