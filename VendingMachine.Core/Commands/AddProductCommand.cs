namespace VendingMachine.Core.Commands
{
    using MediatR;
    using VendingMachine.Core.Models;

    public class AddProductCommand: IRequest<ProductDto>
    {
        public string? Name { get; set; }
    }
}
