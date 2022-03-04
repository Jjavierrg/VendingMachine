namespace VendingMachine.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using VendingMachine.Entities;
    using VendingMachine.Infrastructure.Core;

    public class VendingUoW : UnitOfWork
    {
        public DbSet<Slot> Slots { get; set; }
        public DbSet<Product> Products { get; set; }

        public VendingUoW(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SlotConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
