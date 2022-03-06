namespace VendingMachine.Api.Controllers
{
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using VendingMachine.Domain.Models;
    using VendingMachine.Domain.Querys;
    using VendingMachine.Entities;
    using VendingMachine.Infrastructure.Core;

    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductSlotDto>
    {
        private readonly IReadRepository<Slot> _repository;
        private readonly IMapper _mapper;

        public GetProductQueryHandler(IReadRepository<Slot> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductSlotDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var queryOptions = new QueryOptions<Slot>
            {
                Includes = (q) => q.Include(x => x.Product)
            };

            var product = await _repository.GetEntityAsync(request.Id, queryOptions);
            return _mapper.Map<ProductSlotDto>(product);
        }
    }
}
