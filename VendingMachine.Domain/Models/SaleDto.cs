namespace VendingMachine.Domain.Models
{
    public class SaleDto
    {
        public ProductSlotDto Product { get; set; }
        public int Quantity { get; set; }
        public int OrderPrice { get; set; }
        public IEnumerable<CoinWithQuantityDto> ChangeCoins { get; set; }
    }
}
