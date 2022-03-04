namespace VendingMachine.Core.Services
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using VendingMachine.Entities;
    using VendingMachine.Infrastructure.Core;

    public class WalletService: IWalletService
    {
        private readonly IRepository<CustomerWalletCoin> _walletRepository;
        private readonly IReadRepository<Coin> _coinRepository;

        public WalletService(IRepository<CustomerWalletCoin> walletRepository, IReadRepository<Coin> coinRepository)
        {
            _walletRepository = walletRepository ?? throw new ArgumentNullException(nameof(walletRepository));
            _coinRepository = coinRepository ?? throw new ArgumentNullException(nameof(coinRepository));
        }


        public async Task AddCoinsToCustomerWalletAsync(IEnumerable<(int value, int quantity)> coins)
        {
            var walletCoins = await GetCustomerWalletAsync();
            var validCoins = await _coinRepository.GetListAsync();

            foreach (var group in coins.GroupBy(x => x.value))
            {
                var numberOfCoins = group.Sum(x => x.quantity);
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

        public async Task<int> GetCustomerCreditAsync()
        {
            var walletCoins = await GetCustomerWalletAsync();
            return walletCoins?.Sum(x => x.NumberOfCoins * (x.Coin?.Value ?? 0)) ?? 0;
        }

        public Task<IEnumerable<CustomerWalletCoin>> GetCustomerWalletAsync()
        {
            var options = new QueryOptions<CustomerWalletCoin>
            {
                Includes = q => q.Include(x => x.Coin)
            };

            return _walletRepository.GetListAsync(options);
        }

        public async Task ClearCustomerWalletAsync()
        {
            var wallet = await GetCustomerWalletAsync();
            await RemoveCustomerWalletAsync(wallet);
        }

        public Task RemoveCustomerWalletAsync(IEnumerable<CustomerWalletCoin> coins)
        {
            _walletRepository.RemoveRange(coins);
            return _walletRepository.SaveChangesAsync();
        }
    }
}
