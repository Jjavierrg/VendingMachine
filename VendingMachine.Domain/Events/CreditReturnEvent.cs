namespace VendingMachine.Domain.Events
{
    using MediatR;
    using VendingMachine.Domain.Models;

    public class CreditReturnEvent : INotification
    {
        public CreditReturnEvent(IEnumerable<CoinWithQuantityDto> returnCoins)
        {
            ReturnCoins = returnCoins;
        }

        public IEnumerable<CoinWithQuantityDto> ReturnCoins { get; }
    }
}
