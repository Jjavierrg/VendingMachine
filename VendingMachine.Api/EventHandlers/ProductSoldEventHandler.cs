namespace VendingMachine.Api.EventHandlers
{
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;
    using VendingMachine.Api.Services;
    using VendingMachine.Domain.Events;

    public class ProductSoldEventHandler : INotificationHandler<ProductSoldEvent>
    {
        private INotificationService _notificationService;
        public ProductSoldEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }
        public async Task Handle(ProductSoldEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.NotifySale(notification.Sale);
            await _notificationService.NotifyDisplayAsync("Thank you", 0);
        }
    }
}
