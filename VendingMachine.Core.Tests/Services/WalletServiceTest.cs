namespace VendingMachine.Core.Tests
{
    using System.Linq;
    using System.Threading.Tasks;
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
            _walletService = new WalletService(_dataBaseMock.CustomerWalletCoinRepository, _dataBaseMock.CoinRepository, MapperMock.Mapper);
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
    }
}