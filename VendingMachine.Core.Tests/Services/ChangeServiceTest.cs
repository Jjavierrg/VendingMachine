namespace VendingMachine.Core.Tests
{
    using System.Linq;
    using System.Threading.Tasks;
    using VendingMachine.Core.Exceptions;
    using VendingMachine.Core.Models;
    using VendingMachine.Core.Services;
    using VendingMachine.Core.Tests.Shared;
    using VendingMachine.Entities;
    using Xunit;

    public class ChangeServiceTest
    {
        private readonly IChangeService _changeService;
        private readonly ContextMoq _dataBaseMock;
        private readonly RepositoryMoq<MachineWalletCoin> _emptyMachineRepository;

        public ChangeServiceTest()
        {
            _dataBaseMock = new ContextMoq();
            _emptyMachineRepository = new RepositoryMoq<MachineWalletCoin>();
            _changeService = new ChangeService(_dataBaseMock.CustomerWalletCoinRepository, _emptyMachineRepository, MapperMock.Mapper);
        }

        [Fact]
        public async Task ShouldReturnMinimumCoins()
        {
            // Arrange
            var coins = new MachineWalletCoin[] {
                new MachineWalletCoin { Id = 1, CoinId = 1, Coin = new Coin { Id = 1, Value = 10 }, NumberOfCoins = 2 },
                new MachineWalletCoin { Id = 2, CoinId = 2, Coin = new Coin { Id = 2, Value = 30 }, NumberOfCoins = 3 },
                new MachineWalletCoin { Id = 3, CoinId = 3, Coin = new Coin { Id = 3, Value = 60 }, NumberOfCoins = 4 },
                new MachineWalletCoin { Id = 4, CoinId = 4, Coin = new Coin { Id = 4, Value = 100 }, NumberOfCoins = 9 },
            };
            _emptyMachineRepository.AddRange(coins);

            var shouldList = new CoinWithQuantityDto[] {
                new CoinWithQuantityDto { CoinValue = 60, Quantity = 2 },
            };

            // Act
            var changeCoins = await _changeService.GetCoinChangeAsync(120);

            // Assert
            Assert.True(shouldList.All(s => changeCoins.Any(i => i.CoinValue == s.CoinValue && i.Quantity == s.Quantity)));
        }

        [Fact]
        public async Task ShouldReturnOnlyAvailableCoins()
        {
            // Arrange
            var coins = new MachineWalletCoin[] {
                new MachineWalletCoin { Id = 1, CoinId = 1, Coin = new Coin { Id = 1, Value = 10 }, NumberOfCoins = 1 },
                new MachineWalletCoin { Id = 2, CoinId = 2, Coin = new Coin { Id = 2, Value = 30 }, NumberOfCoins = 3 },
                new MachineWalletCoin { Id = 3, CoinId = 3, Coin = new Coin { Id = 3, Value = 60 }, NumberOfCoins = 0 },
                new MachineWalletCoin { Id = 4, CoinId = 4, Coin = new Coin { Id = 4, Value = 100 }, NumberOfCoins = 9 },
                new MachineWalletCoin { Id = 5, CoinId = 1, Coin = new Coin { Id = 1, Value = 10 }, NumberOfCoins = 1 },
            };
            _emptyMachineRepository.AddRange(coins);

            var shouldList = new CoinWithQuantityDto[] {
                new CoinWithQuantityDto { CoinValue = 10, Quantity = 2 },
                new CoinWithQuantityDto { CoinValue = 100, Quantity = 1 },
            };

            // Act
            var changeCoins = await _changeService.GetCoinChangeAsync(120);

            // Assert
            Assert.True(shouldList.All(s => changeCoins.Any(i => i.CoinValue == s.CoinValue && i.Quantity == s.Quantity)));
        }

        [Fact]
        public async Task ShouldNotReturnIfNotAvailableCoins()
        {
            // Arrange
            var coins = new MachineWalletCoin[] {
                new MachineWalletCoin { Id = 2, CoinId = 2, Coin = new Coin { Id = 2, Value = 20 }, NumberOfCoins = 10 },
                new MachineWalletCoin { Id = 3, CoinId = 3, Coin = new Coin { Id = 3, Value = 60 }, NumberOfCoins = 10 },
                new MachineWalletCoin { Id = 4, CoinId = 4, Coin = new Coin { Id = 4, Value = 100 }, NumberOfCoins = 10 },
            };
            _emptyMachineRepository.AddRange(coins);

            // Act
            var changeAction = () => _changeService.GetCoinChangeAsync(110);

            // Assert
            var caughtException = await Assert.ThrowsAsync<InsufficientChangeException>(changeAction);
            Assert.Equal("Insufficient change. Please insert exact credit", caughtException.Message);
        }
    }
}