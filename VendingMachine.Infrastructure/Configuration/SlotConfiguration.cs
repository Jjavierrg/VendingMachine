namespace VendingMachine.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using VendingMachine.Entities;

    public class SlotConfiguration : BaseConfiguration<Slot>
    {
        public override string Table => "Slots";

        public override void ConfigureEntity(EntityTypeBuilder<Slot> builder)
        {
            builder.Property(x => x.ProductId).HasColumnName(nameof(Slot.ProductId)).IsRequired();
            builder.Property(x => x.Quantity).HasColumnName(nameof(Slot.Quantity)).IsRequired();
            builder.Property(x => x.Price).HasColumnName(nameof(Slot.Price)).IsRequired();

            builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId);
        }
    }
}
