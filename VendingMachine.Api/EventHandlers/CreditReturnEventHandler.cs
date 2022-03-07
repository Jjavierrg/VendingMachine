namespace VendingMachine.Api.EventHandlers
{
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;
    using VendingMachine.Api.Services;
    using VendingMachine.Domain.Events;

    public class CreditReturnEventHandler : INotificationHandler<CreditReturnEvent>
    {
        private INotificationService _notificationService;
        public CreditReturnEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }
        public async Task Handle(CreditReturnEvent notification, CancellationToken cancellationToken)
        {
            if (notification?.ReturnCoins == null || !notification.ReturnCoins.Any())
                return;

            await _notificationService.NotifyDisplayAsync("Take your coins", 0);
            await _notificationService.NotifyCoinsToReturn(notification.ReturnCoins);
        }
    }
}
