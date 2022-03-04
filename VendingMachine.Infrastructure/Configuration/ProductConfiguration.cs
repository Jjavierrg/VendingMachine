namespace VendingMachine.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using VendingMachine.Entities;

    public class ProductConfiguration : BaseConfiguration<Product>
    {
        public override string Table => "Products";

        public override void ConfigureEntity(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name).HasColumnName(nameof(Product.Name)).IsRequired().HasMaxLength(200);
        }
    }
}
