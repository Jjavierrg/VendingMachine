namespace VendingMachine.Core.Models
{
    public class SellDto
    {
        public ProductSlotDto Product { get; set; }
        public int Quantity { get; set; }
        public int OrderPrice { get; set; }
        public IEnumerable<CoinWithQuantityDto> ChangeCoins { get; set; }
    }
}
