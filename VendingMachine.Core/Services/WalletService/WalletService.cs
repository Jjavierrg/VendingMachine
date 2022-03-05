namespace VendingMachine.Core.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using VendingMachine.Core.Models;
    using VendingMachine.Entities;
    using VendingMachine.Infrastructure.Core;

    public class WalletService: IWalletService
    {
        private readonly IRepository<CustomerWalletCoin> _walletRepository;
        private readonly IReadRepository<Coin> _coinRepository;
        private readonly IMapper _mapper;

        public WalletService(IRepository<CustomerWalletCoin> walletRepository, IReadRepository<Coin> coinRepository, IMapper mapper)
        {
            _walletRepository = walletRepository ?? throw new ArgumentNullException(nameof(walletRepository));
            _coinRepository = coinRepository ?? throw new ArgumentNullException(nameof(coinRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task AddCoinsToCustomerWalletAsync(IEnumerable<CoinWithQuantityDto> coins)
        {
            var walletCoins = await GetWalletEntities();
            var validCoins = await _coinRepository.GetListAsync();

            foreach (var group in coins.GroupBy(x => x.CoinValue))
            {
                var numberOfCoins = group.Sum(x => x.Quantity);
                var coinId = validCoins.FirstOrDefault(x => x.Value == group.Key)?.Id ?? 0;

                var walletEntry = walletCoins.FirstOrDefault(x => x.CoinId == coinId);
                if (walletEntry == null)
                {
                    _walletRepository.Add(new CustomerWalletCoin { CoinId = coinId, NumberOfCoins = numberOfCoins });
                }
                else
                {
                    walletEntry.NumberOfCoins += numberOfCoins;
                    _walletRepository.Update(walletEntry);
                }
            }

            await _walletRepository.SaveChangesAsync();
        }

        public async Task<UserCreditDto> GetCustomerCreditAsync()
        {
            var walletCoins = await GetCustomerWalletAsync();
            var credit = walletCoins?.Sum(x => x.Quantity * x.CoinValue) ?? 0;
            return new UserCreditDto { Credit = credit };
        }

        public async Task<IEnumerable<CoinWithQuantityDto>> GetCustomerWalletAsync()
        {
            var coins = await GetWalletEntities();
            return _mapper.Map<IEnumerable<CustomerWalletCoin>, IEnumerable<CoinWithQuantityDto>>(coins);
        }

        public async Task ClearCustomerWalletAsync()
        {
            var wallet = await GetWalletEntities();
            _walletRepository.RemoveRange(wallet);
            await _walletRepository.SaveChangesAsync();
        }

        private Task<IEnumerable<CustomerWalletCoin>> GetWalletEntities()
        {
            var options = new QueryOptions<CustomerWalletCoin>
            {
                Includes = q => q.Include(x => x.Coin)
            };

            return _walletRepository.GetListAsync(options);
        }
    }
}
