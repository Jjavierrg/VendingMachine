namespace VendingMachine.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using VendingMachine.Infrastructure.Core;

    public class VendingUoW : UnitOfWork
    {
        public VendingUoW(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
