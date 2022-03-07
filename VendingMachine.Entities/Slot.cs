namespace VendingMachine.Entities
{
    public class Slot: BaseEntity
    {
        public int Price { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public virtual Product Product { get; set; }
    }
}
