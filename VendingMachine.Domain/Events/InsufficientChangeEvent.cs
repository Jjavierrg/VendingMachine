namespace VendingMachine.Domain.Events
{
    using MediatR;

    public class InsufficientChangeEvent : INotification
    {
        public InsufficientChangeEvent(int changeAmount)
        {
            ChangeAmount = changeAmount;
        }

        public int ChangeAmount { get; }
    }
}
