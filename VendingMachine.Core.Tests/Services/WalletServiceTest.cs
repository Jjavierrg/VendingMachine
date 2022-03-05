namespace VendingMachine.Core.Tests
{
    using System.Linq;
    using System.Threading.Tasks;
    using VendingMachine.Core.Exceptions;
    using VendingMachine.Core.Models;
    using VendingMachine.Core.Services;
    using VendingMachine.Core.Tests.Shared;
    using Xunit;

    public class WalletServiceTest
    {
        private readonly ContextMoq _dataBaseMock;
        private readonly WalletService _walletService;

        public WalletServiceTest()
        {
            _dataBaseMock = new ContextMoq();
            _walletService = new WalletService(_dataBaseMock.CustomerWalletCoinRepository, _dataBaseMock.MachineWalletCoinRepository, _dataBaseMock.CoinRepository, MapperMock.Mapper);
        }

        [Fact]
        public async Task ShouldAddValidCoinsToWallets()
        {
            // Arrange
            CoinWithQuantityDto[] coins = {
                new CoinWithQuantityDto { CoinValue = 10, Quantity = 3 },
                new CoinWithQuantityDto { CoinValue = 10, Quantity = 1 },
                new CoinWithQuantityDto { CoinValue = 20, Quantity = 2 },
                new CoinWithQuantityDto { CoinValue = 50, Quantity = 1 },
                new CoinWithQuantityDto { CoinValue = 100, Quantity = 1 },
            };

            // Act
            await _walletService.AddCoinsToCustomerWalletAsync(coins);
            var walletCoins = await _walletService.GetCustomerWalletAsync();

            // Assert
            Assert.NotNull(walletCoins);
            Assert.NotEmpty(walletCoins);
            Assert.Equal(4, walletCoins.Count());
            Assert.Contains(walletCoins, x => x.Quantity == 4);
        }

        [Fact]
        public async Task ShouldClearCustomerWalletAsync()
        {
            // Arrange
            CoinWithQuantityDto[] coins = {
                new CoinWithQuantityDto { CoinValue = 10, Quantity = 3 },
                new CoinWithQuantityDto { CoinValue = 10, Quantity = 1 },
                new CoinWithQuantityDto { CoinValue = 20, Quantity = 2 },
                new CoinWithQuantityDto { CoinValue = 50, Quantity = 1 },
                new CoinWithQuantityDto { CoinValue = 100, Quantity = 1 },
            };

            // Act
            await _walletService.AddCoinsToCustomerWalletAsync(coins);
            var walletCoinsBeforeClear = await _walletService.GetCustomerWalletAsync();
            await _walletService.ClearCustomerWalletAsync();
            var walletCoinsAfterClear = await _walletService.GetCustomerWalletAsync();

            // Assert
            Assert.NotNull(walletCoinsBeforeClear);
            Assert.NotEmpty(walletCoinsBeforeClear);
            Assert.Empty(walletCoinsAfterClear);
        }

        [Fact]
        public async Task ShouldGetCoinsFromMachineWallet()
        {
            // Arrange
            CoinWithQuantityDto[] coins = {
                new CoinWithQuantityDto { CoinValue = 10, Quantity = 3 },
                new CoinWithQuantityDto { CoinValue = 10, Quantity = 1 },
                new CoinWithQuantityDto { CoinValue = 20, Quantity = 2 },
                new CoinWithQuantityDto { CoinValue = 50, Quantity = 1 },
                new CoinWithQuantityDto { CoinValue = 100, Quantity = 10 },
            };

            // Act
            var result = await _walletService.GetCoinsFromMachineWalletAsync(coins);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ShouldNoGetCoinsFromMachineWalletIfNoCoinsAvailable()
        {
            // Arrange
            CoinWithQuantityDto[] coins = {
                new CoinWithQuantityDto { CoinValue = 100, Quantity = 110 }
            };

            // Act
            var action = () => _walletService.GetCoinsFromMachineWalletAsync(coins);

            // Assert
            var caughtException = await Assert.ThrowsAsync<InsufficientChangeException>(action);
            Assert.Equal("Insufficient change. Please insert exact credit", caughtException.Message);
        }

        [Fact]
        public async Task ShouldMoveCustomerWalletToMachineWallet()
        {
            // Arrange
            var quantityAdded = 110;
            CoinWithQuantityDto[] coins = {
                new CoinWithQuantityDto { CoinValue = 100, Quantity = quantityAdded }
            };

            // Act
            var previousCoins = (await _dataBaseMock.MachineWalletCoinRepository.GetEntityAsync(4)).NumberOfCoins;
            await _walletService.AddCoinsToCustomerWalletAsync(coins);
            await _walletService.MoveCustomerWalletToMachineWalletAsync();
            var customerCoins = await _dataBaseMock.CustomerWalletCoinRepository.GetCountAsync();
            var currentCoins = await _dataBaseMock.MachineWalletCoinRepository.GetEntityAsync(4);

            // Assert
            Assert.Equal(previousCoins + quantityAdded, currentCoins.NumberOfCoins);
            Assert.Equal(0, customerCoins);
        }
    }
}