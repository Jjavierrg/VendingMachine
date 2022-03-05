namespace VendingMachine.Core.Tests
{
    using FluentValidation;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using VendingMachine.Core.Commands;
    using VendingMachine.Core.Commands.Handlers;
    using VendingMachine.Core.Commands.Validators;
    using VendingMachine.Core.Models;
    using VendingMachine.Core.Services;
    using VendingMachine.Core.Tests.Shared;
    using VendingMachine.Entities;
    using Xunit;

    public class InsertCoinsCommandTest
    {
        private readonly ContextMoq _dataBaseMock;
        private readonly IValidator<InsertCoinsCommand> _validator;
        private readonly WalletService _walletService;

        public InsertCoinsCommandTest()
        {
            _dataBaseMock = new ContextMoq();
            _validator = new InsertCoinsCommandValidator(_dataBaseMock.CoinRepository);
            _walletService = new WalletService(_dataBaseMock.CustomerWalletCoinRepository, _dataBaseMock.MachineWalletCoinRepository, _dataBaseMock.CoinRepository, MapperMock.Mapper);
        }

        [Fact]
        public async Task ShouldNotAddInvalidCoinsToWallet()
        {
            // Arrange
            int[] coins = { 10, 10, 10, 22, 20, 50, 100 };
            var walletCoins = coins.Select(x => new CoinWithQuantityDto { CoinValue = x, Quantity = 1 }).ToArray();
            var command = new InsertCoinsCommand(walletCoins);

            // Act
            var valid = await _validator.ValidateAsync(command, options => options.IncludeRuleSets("ValidCoins"));

            // Assert
            Assert.False(valid.IsValid);
        }

        [Fact]
        public async Task ShouldReturnCoinsCommand()
        {
            // Arrange
            var command = new ReturnUserCoinsCommand();
            var logger = new MockupLogger<InsertCoinsHandler>();
            var handler = new ReturnUserCoinsHandler(logger, _walletService);
            var customerCoins = new CustomerWalletCoin[] {
                new CustomerWalletCoin { CoinId = 1, NumberOfCoins = 2 },
                new CustomerWalletCoin { CoinId = 2, NumberOfCoins = 3 },
                new CustomerWalletCoin { CoinId = 3, NumberOfCoins = 4 },
                new CustomerWalletCoin { CoinId = 4, NumberOfCoins = 9 },
            };

            // Act
            _dataBaseMock.CustomerWalletCoinRepository.AddRange(customerCoins);
            var coinsFirstTime = await handler.Handle(command, new CancellationToken());
            var coinsSecondTime = await handler.Handle(command, new CancellationToken());

            // Assert
            Assert.NotEmpty(coinsFirstTime);
            Assert.Empty(coinsSecondTime);
            Assert.Equal(customerCoins.Sum(x => x.NumberOfCoins), coinsFirstTime.Sum(x => x.Quantity));
        }
    }
}