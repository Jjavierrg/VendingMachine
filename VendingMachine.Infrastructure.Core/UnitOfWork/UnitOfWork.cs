namespace VendingMachine.Infrastructure.Core
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.Storage;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class UnitOfWork : DbContext, IUnitOfWork
    {
        protected UnitOfWork(DbContextOptions options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<TEntity> CreateSet<TEntity>() where TEntity : class
        {
            return Set<TEntity>();
        }

        public void Modify<TEntity>(TEntity item) where TEntity : class
        {
            Entry(item).State = EntityState.Modified;
        }

        public async Task SaveChangesAsync()
        {
            IDbContextTransaction? transaction = null;
            try
            {
                transaction = await Database.BeginTransactionAsync();
                _ = await base.SaveChangesAsync();

                if (transaction != null)
                    await transaction.CommitAsync();
            }
            catch (Exception)
            {
                if (transaction != null)
                    await Database.RollbackTransactionAsync();
                throw;
            }
        }

        public void RollbackChanges()
        {
            ChangeTracker.Entries()?.ToList().ForEach(x => x.State = EntityState.Unchanged);
        }

        public void ResetContextState()
        {
            ChangeTracker.Entries()?.Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);
        }

        public EntityEntry<TEntity> GetEntry<TEntity>(TEntity item) where TEntity : class
        {
            return Entry(item);
        }
    }
}
