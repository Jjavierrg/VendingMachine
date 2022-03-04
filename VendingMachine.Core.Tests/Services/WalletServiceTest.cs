namespace VendingMachine.Core.Tests
{
    using System.Linq;
    using System.Threading.Tasks;
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
            _walletService = new WalletService(_dataBaseMock.CustomerWalletCoinRepository, _dataBaseMock.CoinRepository);
        }

        [Fact]
        public async Task AddValidCoinsToWallets()
        {
            // Arrange
            int[] coins = { 10, 10, 10, 20, 20, 50, 100 };
            var databaseCoins = await _dataBaseMock.CoinRepository.GetListAsync();

            // Act
            await _walletService.AddCoinsToCustomerWalletAsync(coins.Select(x => (x, 1)));
            var walletCoins = await _walletService.GetCustomerWalletAsync();
            var walletCredit = walletCoins?.Sum(x => {
                var coin = databaseCoins.First(c => c.Id == x.CoinId);
                return coin.Value * x.NumberOfCoins;
            }) ?? 0;

            // Assert
            Assert.NotNull(walletCoins);
            Assert.NotEmpty(walletCoins);
            Assert.Equal(coins.Sum(), walletCredit);
        }
    }
}