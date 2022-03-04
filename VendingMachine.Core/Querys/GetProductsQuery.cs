namespace VendingMachine.Core.Querys
{
    using MediatR;
    using System.Collections.Generic;
    using VendingMachine.Core.Models;

    public class GetProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
    }
}
