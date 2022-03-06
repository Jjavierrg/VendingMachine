namespace VendingMachine.Domain.Services
{
    using VendingMachine.Domain.Models;

    public interface IWalletService
    {
        Task AddCoinsToCustomerWalletAsync(IEnumerable<CoinWithQuantityDto> coins);
        Task ClearCustomerWalletAsync();
        Task<bool> GetCoinsFromMachineWalletAsync(IEnumerable<CoinWithQuantityDto> coinsRequested);
        Task<UserCreditDto> GetCustomerCreditAsync();
        Task<IEnumerable<CoinWithQuantityDto>> GetCustomerWalletAsync();
        Task MoveCustomerWalletToMachineWalletAsync();
    }
}