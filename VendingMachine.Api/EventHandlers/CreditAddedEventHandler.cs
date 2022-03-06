namespace VendingMachine.Api.EventHandlers
{
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;
    using VendingMachine.Api.Services;
    using VendingMachine.Domain.Events;

    public class CreditAddedEventHandler : INotificationHandler<CreditAddedEvent>
    {
        private INotificationService _notificationService;
        public CreditAddedEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }
        public Task Handle(CreditAddedEvent notification, CancellationToken cancellationToken)
        {
            return _notificationService.NotifyDisplayAsync("", notification.NewUserCredit);
        }
    }
}
