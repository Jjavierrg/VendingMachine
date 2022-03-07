namespace VendingMachine.Domain.Querys
{
    using MediatR;
    using VendingMachine.Domain.Models;

    public class GetProductQuery : IRequest<ProductSlotDto>
    {
        public GetProductQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
