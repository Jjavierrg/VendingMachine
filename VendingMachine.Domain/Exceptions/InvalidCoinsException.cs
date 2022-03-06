namespace VendingMachine.Domain.Exceptions
{
    using System;

    public class InvalidCoinsException : Exception
    {
        public InvalidCoinsException(string message) : base(message)
        {
        }

        public InvalidCoinsException() : this("Invalid Coins")
        {
        }
    }
}
