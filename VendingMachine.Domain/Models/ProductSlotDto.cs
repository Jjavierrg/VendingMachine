namespace VendingMachine.Domain.Models
{
    public class ProductSlotDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
