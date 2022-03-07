namespace VendingMachine.Api.Controllers
{
    using AutoMapper;
    using MediatR;
    using VendingMachine.Domain.Models;
    using VendingMachine.Domain.Querys;
    using VendingMachine.Domain.Services;
    using VendingMachine.Entities;
    using VendingMachine.Infrastructure.Core;

    public class GetAvailableCoinsQueryHandler : IRequestHandler<GetAvailableCoinsQuery, IEnumerable<CoinWithQuantityDto>>
    {
        private readonly IReadRepository<Coin> _coinRepository;
        private readonly IMapper _mapper;

        public GetAvailableCoinsQueryHandler(IReadRepository<Coin> coinRepository, IMapper mapper)
        {
            _coinRepository = coinRepository ?? throw new ArgumentNullException(nameof(coinRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<CoinWithQuantityDto>> Handle(GetAvailableCoinsQuery request, CancellationToken cancellationToken)
        {
            var coins = await _coinRepository.GetListAsync();
            return _mapper.Map<IEnumerable<CoinWithQuantityDto>>(coins);
        }
    }
}
