namespace VendingMachine.Infrastructure.Core
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using VendingMachine.Entities;

    public class Repository<T> : ReadRepository<T>, IRepository<T> where T : BaseEntity
    {
        public Repository(IUnitOfWork unitOfWork, ILogger<Repository<T>> logger) : base(unitOfWork, logger) { }

        public virtual async Task SaveChangesAsync()
        {
            try
            {
                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, ex.Message);
                throw;
            }
        }

        public virtual void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            EntitySet.Add(item);
        }

        /// <inheritdoc />
        public virtual void AddRange(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            EntitySet.AddRange(items);
        }

        /// <inheritdoc />
        public virtual void Update(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            EntitySet.Attach(item);
            UnitOfWork.Modify(item);
        }

        /// <inheritdoc />
        public virtual void Remove(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (UnitOfWork.GetEntry(item).State == EntityState.Detached)
                EntitySet.Attach(item);

            EntitySet.Remove(item);
        }

        public virtual void RemoveRange(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
                Remove(item);
        }
    }
}
