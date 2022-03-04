namespace VendingMachine.Api.Controllers
{
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using VendingMachine.Core.Models;
    using VendingMachine.Core.Querys;
    using VendingMachine.Entities;
    using VendingMachine.Infrastructure.Core;

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IReadRepository<Slot> _repository;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(IReadRepository<Slot> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var queryOptions = new QueryOptions<Slot>
            {
                Includes = (q) => q.Include(x => x.Product)
            };

            if (request.HideNoStock)
                queryOptions.Filter = (x) => x.Quantity > 0;

            var entities = await _repository.GetListAsync(queryOptions);
            return _mapper.Map<IEnumerable<ProductDto>>(entities);
        }
    }
}
