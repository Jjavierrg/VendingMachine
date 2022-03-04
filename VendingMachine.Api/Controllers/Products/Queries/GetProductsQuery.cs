namespace VendingMachine.Api.Controllers
{
    using AutoMapper;
    using MediatR;
    using VendingMachine.Entities;
    using VendingMachine.Infrastructure.Core;

    public class GetProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
    }

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IReadRepository<Product> _repository;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(IReadRepository<Product> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetListAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(entities);
        }
    }
}
