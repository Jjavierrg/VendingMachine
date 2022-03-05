namespace VendingMachine.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VendingMachine.Core.Models;

    public interface IChangeService
    {
        Task<IEnumerable<CoinWithQuantityDto>> GetCoinChangeAsync(int changeAmount);
    }
}