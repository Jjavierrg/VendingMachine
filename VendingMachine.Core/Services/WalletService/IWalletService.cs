namespace VendingMachine.Core.Services
{
    using VendingMachine.Entities;

    public interface IWalletService
    {
        Task AddCoinsToCustomerWalletAsync(IEnumerable<(int value, int quantity)> coins);
        Task ClearCustomerWalletAsync();
        Task<int> GetCustomerCreditAsync();
        Task<IEnumerable<CustomerWalletCoin>> GetCustomerWalletAsync();
        Task RemoveCustomerWalletAsync(IEnumerable<CustomerWalletCoin> coins);
    }
}