namespace VendingMachine.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using VendingMachine.Entities;

    public class CoinConfiguration : BaseConfiguration<Coin>
    {
        public override string Table => "Coins";

        public override void ConfigureEntity(EntityTypeBuilder<Coin> builder)
        {
            builder.Property(x => x.Description).HasColumnName(nameof(Coin.Description)).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Value).HasColumnName(nameof(Coin.Value)).IsRequired();
        }
    }
}
