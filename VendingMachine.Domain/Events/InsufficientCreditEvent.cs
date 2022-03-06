namespace VendingMachine.Domain.Events
{
    using MediatR;

    public class InsufficientCreditEvent : INotification
    {
        public InsufficientCreditEvent(int salePrice, int creditAvailable)
        {
            SalePrice = salePrice;
            CreditAvailable = creditAvailable;
        }

        public int SalePrice { get; }
        public int CreditAvailable { get; }
        public int Reaiming => SalePrice - CreditAvailable;
    }
}
