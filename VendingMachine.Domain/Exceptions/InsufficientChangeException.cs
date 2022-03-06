namespace VendingMachine.Domain.Exceptions
{
    using System;

    public class InsufficientChangeException : Exception
    {
        public InsufficientChangeException(string message) : base(message)
        {
        }

        public InsufficientChangeException() : this("Insufficient change. Please insert exact credit")
        {
        }
    }
}
