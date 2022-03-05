namespace VendingMachine.Core.Services
{
    using System.Threading.Tasks;
    using VendingMachine.Core.Models;

    public interface ISellService
    {
        Task<ProductSlotDto> DiscountQuantityAndGetNewStock(int slotId, int quantityOrdered);
        Task<int> GetOrderPrice(int slotId, int quantityOrdered);
        Task<bool> SlotHasEnoughStock(int slotId, int quantityOrdered);
    }
}