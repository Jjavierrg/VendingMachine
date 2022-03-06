namespace VendingMachine.Domain.Events
{
    using MediatR;
    using VendingMachine.Domain.Models;

    public class CreditAddedEvent : INotification
    {
        public CreditAddedEvent(UserCreditDto newUserCredit)
        {
            NewUserCredit = newUserCredit;
        }

        public UserCreditDto NewUserCredit { get; }
    }
}
