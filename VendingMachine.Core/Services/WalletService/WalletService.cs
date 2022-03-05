namespace VendingMachine.Core.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using VendingMachine.Core.Exceptions;
    using VendingMachine.Core.Models;
    using VendingMachine.Entities;
    using VendingMachine.Infrastructure.Core;

    public class WalletService: IWalletService
    {
        private readonly IRepository<CustomerWalletCoin> _customerWalletRepository;
        private readonly IRepository<MachineWalletCoin> _machineWalletRepository;
        private readonly IReadRepository<Coin> _coinRepository;
        private readonly IMapper _mapper;

        public WalletService(IRepository<CustomerWalletCoin> customerWalletRepository, IRepository<MachineWalletCoin> machineWalletRepository, IReadRepository<Coin> coinRepository, IMapper mapper)
        {
            _customerWalletRepository = customerWalletRepository ?? throw new ArgumentNullException(nameof(customerWalletRepository));
            _machineWalletRepository = machineWalletRepository ?? throw new ArgumentNullException(nameof(machineWalletRepository));
            _coinRepository = coinRepository ?? throw new ArgumentNullException(nameof(coinRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task AddCoinsToCustomerWalletAsync(IEnumerable<CoinWithQuantityDto> coins)
        {
            var customerWallet = await GetCustomerWalletEntities();
            var validCoins = await _coinRepository.GetListAsync();

            foreach (var group in coins.GroupBy(x => x.CoinValue))
            {
                var numberOfCoins = group.Sum(x => x.Quantity);
                var coinId = validCoins.FirstOrDefault(x => x.Value == group.Key)?.Id ?? 0;

                var walletEntry = customerWallet.FirstOrDefault(x => x.CoinId == coinId);
                if (walletEntry == null)
                {
                    _customerWalletRepository.Add(new CustomerWalletCoin { CoinId = coinId, NumberOfCoins = numberOfCoins });
                }
                else
                {
                    walletEntry.NumberOfCoins += numberOfCoins;
                    _customerWalletRepository.Update(walletEntry);
                }
            }

            await _customerWalletRepository.SaveChangesAsync();
        }

        public async Task<UserCreditDto> GetCustomerCreditAsync()
        {
            var customerWalletCoins = await GetCustomerWalletAsync();
            var credit = customerWalletCoins?.Sum(x => x.Quantity * x.CoinValue) ?? 0;
            return new UserCreditDto { Credit = credit };
        }

        public async Task<IEnumerable<CoinWithQuantityDto>> GetCustomerWalletAsync()
        {
            var coins = await GetCustomerWalletEntities();
            return _mapper.Map<IEnumerable<CustomerWalletCoin>, IEnumerable<CoinWithQuantityDto>>(coins);
        }

        public async Task ClearCustomerWalletAsync()
        {
            var customerWallet = await _customerWalletRepository.GetListAsync();
            _customerWalletRepository.RemoveRange(customerWallet);
            await _customerWalletRepository.SaveChangesAsync();
        }

        public async Task MoveCustomerWalletToMachineWalletAsync()
        {
            var customerWallet = await GetCustomerWalletEntities();
            var machineWallet = await GetMachineWalletEntities();

            var coinsEntriesToUpdate = customerWallet.Where(x => machineWallet.Any(c => c.CoinId == x.CoinId)).ToList();
            var coinsEntriesToAdd = customerWallet.Except(coinsEntriesToUpdate).Select(x => new MachineWalletCoin
            {
                CoinId = x.CoinId,
                NumberOfCoins = x.NumberOfCoins
            });

            if (coinsEntriesToUpdate.Any())
                coinsEntriesToUpdate.ForEach(x => {
                    var machineCoin = machineWallet.First(c => c.CoinId == x.CoinId);
                    machineCoin.NumberOfCoins += x.NumberOfCoins;
                    _machineWalletRepository.Update(machineCoin);
                });

            if (coinsEntriesToAdd.Any())
                _machineWalletRepository.AddRange(coinsEntriesToAdd);

            await _machineWalletRepository.SaveChangesAsync();
            await ClearCustomerWalletAsync();
        }

        public async Task<bool> GetCoinsFromMachineWalletAsync(IEnumerable<CoinWithQuantityDto> coinsRequested)
        {
            var coinValues = coinsRequested?.Select(x => x.CoinValue).Distinct() ?? Enumerable.Empty<int>();
            if (!coinValues.Any())
                return true;

            var machineQueryOptions = new QueryOptions<MachineWalletCoin>
            {
                Includes = q => q.Include(x => x.Coin),
                Filter = x => coinValues.Contains(x.Coin.Value)
            };
            var machineWallet = await _machineWalletRepository.GetListAsync(machineQueryOptions);

            coinsRequested.ToList().ForEach(x =>
            {
                var machineCoin = machineWallet.FirstOrDefault(c => c.Coin?.Value == x.CoinValue);
                if (machineCoin == null || machineCoin.NumberOfCoins < x.Quantity)
                    throw new InsufficientChangeException();

                machineCoin.NumberOfCoins -= x.Quantity;
            });

            machineWallet.ToList().ForEach(_machineWalletRepository.Update);
            await _customerWalletRepository.SaveChangesAsync();
            return true;
        }

        private Task<IEnumerable<CustomerWalletCoin>> GetCustomerWalletEntities()
        {
            var options = new QueryOptions<CustomerWalletCoin>
            {
                Includes = q => q.Include(x => x.Coin)
            };

            return _customerWalletRepository.GetListAsync(options);
        }

        private Task<IEnumerable<MachineWalletCoin>> GetMachineWalletEntities()
        {
            var options = new QueryOptions<MachineWalletCoin>
            {
                Includes = q => q.Include(x => x.Coin)
            };

            return _machineWalletRepository.GetListAsync(options);
        }
    }
}
