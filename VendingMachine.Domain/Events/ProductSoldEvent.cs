namespace VendingMachine.Domain.Events
{
    using MediatR;
    using VendingMachine.Domain.Models;

    public class ProductSoldEvent : INotification
    {
        public ProductSoldEvent(SaleDto sale)
        {
            Sale = sale;
        }

        public SaleDto Sale { get; }
    }
}
