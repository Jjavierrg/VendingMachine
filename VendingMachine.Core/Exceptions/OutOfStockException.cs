namespace VendingMachine.Core.Exceptions
{
    using System;

    public class OutOfStockException : Exception
    {
        public OutOfStockException(string message) : base(message)
        {
        }

        public OutOfStockException() : this("Product is out of Stock")
        {
        }
    }
}
