namespace VendingMachine.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using VendingMachine.Entities;

    public abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public abstract string Table { get; }
        public abstract void ConfigureEntity(EntityTypeBuilder<T> builder);

        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.ToTable(Table, "dbo");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id").IsRequired().ValueGeneratedOnAdd();

            ConfigureEntity(builder);
        }
    }
}
