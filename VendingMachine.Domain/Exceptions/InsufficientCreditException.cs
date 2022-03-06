namespace VendingMachine.Domain.Exceptions
{
    using System;

    public class InsufficientCreditException : Exception
    {
        public InsufficientCreditException(string message, int remaining) : base(message)
        {
            Remaining = remaining;
        }

        public InsufficientCreditException(int remaining) : this("Insufficient amount", remaining)
        {
        }

        public int Remaining { get; }
    }
}
