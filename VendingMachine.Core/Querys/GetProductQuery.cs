namespace VendingMachine.Core.Querys
{
    using MediatR;
    using VendingMachine.Core.Models;

    public class GetProductQuery : IRequest<ProductSlotDto>
    {
        public GetProductQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
