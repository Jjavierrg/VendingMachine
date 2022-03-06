namespace VendingMachine.Api.Services
{
    using Microsoft.AspNetCore.SignalR;
    using System.Text.Json;
    using System.Threading.Tasks;
    using VendingMachine.Api.Hubs;
    using VendingMachine.Api.Models;
    using VendingMachine.Domain.Models;

    public class NotificationService: INotificationService
    {
        private IHubContext<Vending> _vendingHub;

        public NotificationService(IHubContext<Vending> vendingHub)
        {
            _vendingHub = vendingHub ?? throw new ArgumentNullException(nameof(vendingHub));
        }

        public Task NotifyDisplayAsync(string statusText, int? credit)
        {
            var data = new DisplayData
            {
                StatusText = statusText,
                CreditInfo = credit
            };

            var json = JsonSerializer.Serialize(data);
            return _vendingHub.Clients.All.SendAsync("display", json);
        }

        public Task NotifyDisplayAsync(string statusText, UserCreditDto? userCredit = null) => NotifyDisplayAsync(statusText, userCredit?.Credit);

        public Task NotifyCoinsToReturn(IEnumerable<CoinWithQuantityDto> coins)
        {
            if (coins == null || !coins.Any())
                return Task.CompletedTask;

            var json = JsonSerializer.Serialize(coins);
            return _vendingHub.Clients.All.SendAsync("returned", json);
        }

        public async Task NotifySale(SaleDto sale)
        {
            if (sale == null)
                return;

            var product = new SaleProduct
            {
                ProductName = sale.Product?.Name,
                Quantity = sale.Quantity
            };

            var productJson = JsonSerializer.Serialize(product);
            await _vendingHub.Clients.All.SendAsync("sale", productJson);

            if (sale.ChangeCoins != null && sale.ChangeCoins.Any())
                await NotifyCoinsToReturn(sale.ChangeCoins);
        }
    }
}
