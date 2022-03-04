namespace VendingMachine.Core.Tests
{
    using FluentValidation;
    using System.Linq;
    using System.Threading.Tasks;
    using VendingMachine.Core.Commands;
    using VendingMachine.Core.Commands.Validators;
    using VendingMachine.Core.Models;
    using VendingMachine.Core.Services;
    using VendingMachine.Core.Tests.Shared;
    using Xunit;

    public class InsertCoinsCommandTest
    {
        private readonly ContextMoq _dataBaseMock;
        private readonly IValidator<InsertCoinsCommand> _validator;

        public InsertCoinsCommandTest()
        {
            _dataBaseMock = new ContextMoq();
            _validator = new InsertCoinsCommandValidator(_dataBaseMock.CoinRepository);
        }

        [Fact]
        public async Task AddInvalidCoinsToWallets()
        {
            // Arrange
            int[] coins = { 10, 10, 10, 22, 20, 50, 100 };
            var walletCoins = coins.Select(x => new WalletCoinDto { CoinValue = x, Quantity = 1 }).ToArray();
            var command = new InsertCoinsCommand(walletCoins);

            // Act
            var valid = _validator.Validate(command);

            // Assert
            Assert.False(valid.IsValid);
        }
    }
}