namespace VendingMachine.Api.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VendingMachine.Domain.Models;

    public interface INotificationService
    {
        Task NotifyCoinsToReturn(IEnumerable<CoinWithQuantityDto> coins);
        Task NotifyDisplayAsync(string statusText, int? credit);
        Task NotifyDisplayAsync(string statusText, UserCreditDto? userCredit = null);
        Task NotifySale(SaleDto sale);
    }
}