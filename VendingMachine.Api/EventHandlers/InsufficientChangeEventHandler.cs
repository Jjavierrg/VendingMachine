namespace VendingMachine.Api.EventHandlers
{
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;
    using VendingMachine.Api.Services;
    using VendingMachine.Domain.Events;

    public class InsufficientChangeEventHandler : INotificationHandler<InsufficientChangeEvent>
    {
        private INotificationService _notificationService;
        public InsufficientChangeEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }
        public Task Handle(InsufficientChangeEvent notification, CancellationToken cancellationToken)
        {
            return _notificationService.NotifyDisplayAsync("No change available!");
        }
    }
}
