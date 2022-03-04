namespace VendingMachine.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using VendingMachine.Entities;

    public class CustomerWalletCoinConfiguration : BaseConfiguration<CustomerWalletCoin>
    {
        public override string Table => "CustomerWalletCoins";

        public override void ConfigureEntity(EntityTypeBuilder<CustomerWalletCoin > builder)
        {
            builder.Property(x => x.CoinId).HasColumnName(nameof(CustomerWalletCoin .CoinId)).IsRequired();
            builder.Property(x => x.NumberOfCoins).HasColumnName(nameof(CustomerWalletCoin .NumberOfCoins)).IsRequired();
            
            builder.HasOne(x => x.Coin).WithMany().HasForeignKey(x => x.CoinId);
        }
    }
}
