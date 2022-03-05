namespace VendingMachine.Api.Controllers
{
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using VendingMachine.Core.Models;
    using VendingMachine.Core.Querys;
    using VendingMachine.Entities;
    using VendingMachine.Infrastructure.Core;

    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
    {
        private readonly IReadRepository<Slot> _repository;
        private readonly IMapper _mapper;

        public GetProductQueryHandler(IReadRepository<Slot> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var queryOptions = new QueryOptions<Slot>
            {
                Includes = (q) => q.Include(x => x.Product)
            };

            var product = await _repository.GetEntityAsync(request.Id, queryOptions);
            return _mapper.Map<ProductDto>(product);
        }
    }
}
