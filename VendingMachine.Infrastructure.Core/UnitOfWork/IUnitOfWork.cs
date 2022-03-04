namespace VendingMachine.Infrastructure.Core
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System;
    using System.Threading.Tasks;

    public interface IUnitOfWork : IDisposable
    {
        DbSet<TEntity> CreateSet<TEntity>() where TEntity : class;
        Task SaveChangesAsync();
        void RollbackChanges();
        void Modify<TEntity>(TEntity item) where TEntity : class;
        EntityEntry<TEntity> GetEntry<TEntity>(TEntity item) where TEntity : class;
    }
}
