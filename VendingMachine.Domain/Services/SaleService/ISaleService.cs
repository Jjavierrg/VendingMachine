namespace VendingMachine.Domain.Services
{
    using System.Threading.Tasks;
    using VendingMachine.Domain.Models;

    public interface ISaleService
    {
        Task<ProductSlotDto> DiscountQuantityAndGetNewStock(int slotId, int quantityOrdered);
        Task<int> GetOrderPrice(int slotId, int quantityOrdered);
        Task<bool> SlotHasEnoughStock(int slotId, int quantityOrdered);
    }
}