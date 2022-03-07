namespace VendingMachine.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using VendingMachine.Entities;

    public class MachineWalletCoinConfiguration : BaseConfiguration<MachineWalletCoin>
    {
        public override string Table => "MachineWalletCoins";

        public override void ConfigureEntity(EntityTypeBuilder<MachineWalletCoin> builder)
        {
            builder.Property(x => x.CoinId).HasColumnName(nameof(MachineWalletCoin.CoinId)).IsRequired();
            builder.Property(x => x.NumberOfCoins).HasColumnName(nameof(MachineWalletCoin.NumberOfCoins)).IsRequired();
            
            builder.HasOne(x => x.Coin).WithMany().HasForeignKey(x => x.CoinId);
        }
    }
}
