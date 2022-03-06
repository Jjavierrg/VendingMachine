namespace VendingMachine.Api.EventHandlers
{
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;
    using VendingMachine.Api.Services;
    using VendingMachine.Domain.Events;

    public class InsufficientCreditEventHandler : INotificationHandler<InsufficientCreditEvent>
    {
        private INotificationService _notificationService;
        public InsufficientCreditEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }
        public Task Handle(InsufficientCreditEvent notification, CancellationToken cancellationToken)
        {
            return _notificationService.NotifyDisplayAsync("Insufficient amount");
        }
    }
}
