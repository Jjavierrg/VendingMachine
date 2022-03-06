namespace VendingMachine.Domain.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using VendingMachine.Domain.Exceptions;
    using VendingMachine.Domain.Models;
    using VendingMachine.Entities;
    using VendingMachine.Infrastructure.Core;

    public class ChangeService : IChangeService
    {
        private readonly IReadRepository<CustomerWalletCoin> _customerWalletRepository;
        private readonly IReadRepository<MachineWalletCoin> _machineWalletRepository;
        private readonly IMapper _mapper;

        public ChangeService(IReadRepository<CustomerWalletCoin> customerWalletRepository, IReadRepository<MachineWalletCoin> machineWalletRepository, IMapper mapper)
        {
            _customerWalletRepository = customerWalletRepository ?? throw new ArgumentNullException(nameof(customerWalletRepository));
            _machineWalletRepository = machineWalletRepository ?? throw new ArgumentNullException(nameof(machineWalletRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<CoinWithQuantityDto>> GetCoinChangeAsync(int changeAmount)
        {
            if (changeAmount == 0)
                return new List<CoinWithQuantityDto>();

            var coins = await GetAvailableCoins();
            var normalizedCoins = NormalizeAvailableCoins(coins);

            if (!normalizedCoins.Any())
                throw new InsufficientChangeException();

            // To simplify, reduce amount dividing by 10
            var coinsValues = normalizedCoins.Select(x => x.CoinValue / 10).ToArray();
            var coinsUnits = normalizedCoins.Select(x => x.Quantity).ToArray();
            var coinsUsed = GetChangeCoinsUnits(changeAmount / 10, coinsValues, coinsUnits);

            if (!coinsUsed.Any(x => x > 0))
                throw new InsufficientChangeException();

            return coinsUsed
                .Select((q, idx) => new CoinWithQuantityDto { CoinValue = coinsValues[idx] * 10, Quantity = q })
                .Where(x => x.Quantity > 0);
        }

        private IEnumerable<CoinWithQuantityDto> NormalizeAvailableCoins(IEnumerable<CoinWithQuantityDto> coins)
        {
            if (coins == null || !coins.Any())
                return coins;

            return coins.GroupBy(x => x.CoinValue)
                .Select(x => new CoinWithQuantityDto { CoinValue = x.Key, Quantity = x.Sum(c => c.Quantity) })
                .OrderBy(x => x.CoinValue);
        }

        /// <summary>
        /// </summary>
        /// <see cref="https://stackoverflow.com/questions/54841133/minimum-coin-change-problem-with-limited-amount-of-coins"/>
        private IEnumerable<int> GetChangeCoinsUnits(int amount, int[] coins, int[] limits)
        {
            int[][] coinsUsed = new int[amount + 1][];
            for (int i = 0; i <= amount; ++i)
            {
                coinsUsed[i] = new int[coins.Length];
            }

            int[] minCoins = new int[amount + 1];
            for (int i = 1; i <= amount; ++i)
            {
                minCoins[i] = int.MaxValue - 1;
            }

            int[] limitsCopy = new int[limits.Length];
            limits.CopyTo(limitsCopy, 0);

            for (int i = 0; i < coins.Length; ++i)
            {
                while (limitsCopy[i] > 0)
                {
                    for (int j = amount; j >= 0; --j)
                    {
                        int currAmount = j + coins[i];
                        if (currAmount <= amount)
                        {
                            if (minCoins[currAmount] > minCoins[j] + 1)
                            {
                                minCoins[currAmount] = minCoins[j] + 1;

                                coinsUsed[j].CopyTo(coinsUsed[currAmount], 0);
                                coinsUsed[currAmount][i] += 1;
                            }
                        }
                    }

                    limitsCopy[i] -= 1;
                }
            }

            if (minCoins[amount] == int.MaxValue - 1)
                return Array.Empty<int>();

            return coinsUsed[amount];
        }

        private async Task<IEnumerable<CoinWithQuantityDto>> GetAvailableCoins()
        {
            var customerQueryOptions = new QueryOptions<CustomerWalletCoin> { Includes = q => q.Include(x => x.Coin) };
            var customerCoinsEntities = await _customerWalletRepository.GetListAsync(customerQueryOptions);
            var customerCoins = _mapper.Map<IEnumerable<CoinWithQuantityDto>>(customerCoinsEntities) ?? Enumerable.Empty<CoinWithQuantityDto>();

            var machineQueryOptions = new QueryOptions<MachineWalletCoin> { Includes = q => q.Include(x => x.Coin) };
            var machineCoinsEntities = await _machineWalletRepository.GetListAsync(machineQueryOptions);
            var machineCoins = _mapper.Map<IEnumerable<CoinWithQuantityDto>>(machineCoinsEntities) ?? Enumerable.Empty<CoinWithQuantityDto>();

            return customerCoins.Union(machineCoins);
        }
    }
}
