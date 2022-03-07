namespace VendingMachine.Domain.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VendingMachine.Domain.Models;

    public interface IChangeService
    {
        Task<IEnumerable<CoinWithQuantityDto>> GetCoinChangeAsync(int changeAmount);
    }
}