namespace VendingMachine.Entities
{
    public class Coin: BaseEntity
    {
        public string? Description { get; set; }
        public int Value { get; set; }
    }
}
