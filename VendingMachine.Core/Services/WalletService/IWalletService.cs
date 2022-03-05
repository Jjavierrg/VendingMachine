namespace VendingMachine.Core.Services
{
    using VendingMachine.Core.Models;
    using VendingMachine.Entities;

    public interface IWalletService
    {
        Task AddCoinsToCustomerWalletAsync(IEnumerable<CoinWithQuantityDto> coins);
        Task ClearCustomerWalletAsync();
        Task<UserCreditDto> GetCustomerCreditAsync();
        Task<IEnumerable<CoinWithQuantityDto>> GetCustomerWalletAsync();
    }
}