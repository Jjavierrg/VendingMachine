namespace VendingMachine.Domain.Commands
{
    using MediatR;
    using VendingMachine.Domain.Models;

    public class SellProductCommand : IRequest<SellDto>
    {
        public SellProductCommand(int quantity, int slotNumber)
        {
            Quantity = quantity;
            SlotNumber = slotNumber;
        }

        public int SlotNumber { get; }
        public int Quantity { get; }
    }
}
