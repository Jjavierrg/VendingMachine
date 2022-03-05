namespace VendingMachine.Core.Querys
{
    using MediatR;
    using VendingMachine.Core.Models;

    public class GetProductsQuery : IRequest<IEnumerable<ProductSlotDto>>
    {
        public GetProductsQuery() : this(false) { }
        public GetProductsQuery(bool hideNoStock)
        {
            HideNoStock = hideNoStock;
        }

        public bool HideNoStock { get; }
    }
}
