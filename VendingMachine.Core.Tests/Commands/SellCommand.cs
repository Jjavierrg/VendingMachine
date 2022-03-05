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

    public class SellCommand
    {
        private readonly ContextMoq _dataBaseMock;
        private readonly IValidator<InsertCoinsCommand> _validator;
        private readonly WalletService _walletService;

        public SellCommand()
        {
            _dataBaseMock = new ContextMoq();
            _validator = new InsertCoinsCommandValidator(_dataBaseMock.CoinRepository);
            _walletService = new WalletService(_dataBaseMock.CustomerWalletCoinRepository, _dataBaseMock.MachineWalletCoinRepository, _dataBaseMock.CoinRepository, MapperMock.Mapper);
        }
    }
}