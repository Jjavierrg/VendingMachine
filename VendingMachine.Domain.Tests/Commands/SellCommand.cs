namespace VendingMachine.Domain.Tests
{
    using FluentValidation;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using VendingMachine.Domain.Commands;
    using VendingMachine.Domain.Commands.Handlers;
    using VendingMachine.Domain.Commands.Validators;
    using VendingMachine.Domain.Models;
    using VendingMachine.Domain.Services;
    using VendingMachine.Domain.Tests.Shared;
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