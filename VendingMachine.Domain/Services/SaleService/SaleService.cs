namespace VendingMachine.Domain.Services
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using VendingMachine.Domain.Exceptions;
    using VendingMachine.Domain.Models;
    using VendingMachine.Entities;
    using VendingMachine.Infrastructure.Core;

    public class SellService : ISaleService
    {
        private readonly IRepository<Slot> _slotRepository;
        private readonly IMapper _mapper;

        public SellService(IRepository<Slot> slotRepository, IMapper mapper)
        {
            _slotRepository = slotRepository ?? throw new ArgumentNullException(nameof(slotRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<bool> SlotHasEnoughStock(int slotId, int quantityOrdered)
        {
            var slot = await _slotRepository.GetEntityAsync(slotId);
            return slot != null && slot.Quantity >= quantityOrdered;
        }

        public async Task<ProductSlotDto> DiscountQuantityAndGetNewStock(int slotId, int quantityOrdered)
        {
            var options = new QueryOptions<Slot>
            {
                Includes = q => q.Include(x => x.Product)
            };

            var slot = await _slotRepository.GetEntityAsync(slotId, options);
            var hasStock = slot != null && slot.Quantity >= quantityOrdered;
            if (!hasStock)
                throw new OutOfStockException();

            slot.Quantity -= quantityOrdered;
            _slotRepository.Update(slot);

            await _slotRepository.SaveChangesAsync();
            return _mapper.Map<ProductSlotDto>(slot);
        }

        public async Task<int> GetOrderPrice(int slotId, int quantityOrdered)
        {
            var slot = await _slotRepository.GetEntityAsync(slotId);
            var unitPrice = slot?.Price ?? 0;

            return unitPrice * quantityOrdered;
        }
    }
}
