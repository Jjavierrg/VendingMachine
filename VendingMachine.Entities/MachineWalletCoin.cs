namespace VendingMachine.Entities
{
    public class MachineWalletCoin: BaseEntity
    {
        public int CoinId { get; set; }
        public int NumberOfCoins { get; set; }

        public virtual Coin Coin { get; set; }
    }
}
