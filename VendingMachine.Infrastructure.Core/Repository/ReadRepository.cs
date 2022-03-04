namespace VendingMachine.Infrastructure.Core
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using VendingMachine.Entities;

    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        public IUnitOfWork UnitOfWork { get; }
        protected readonly ILogger<ReadRepository<T>> Logger;

        public ReadRepository(IUnitOfWork unitOfWork, ILogger<ReadRepository<T>> logger)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public DbSet<T> EntitySet => UnitOfWork.CreateSet<T>();

        /// <inheritdoc />
        public virtual Task<T?> GetEntityAsync(int id, IQueryOptions<T> queryOptions)
        {
            var query = GetQuery(queryOptions);
            return query.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <inheritdoc />
        public virtual Task<T?> GetFirstOrDefaultAsync(IQueryOptions<T> queryOptions)
        {
            var query = GetQuery(queryOptions);
            return query.FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<T>> GetListAsync(IQueryOptions<T> queryOptions)
        {
            var query = GetQuery(queryOptions);
            return await query.ToListAsync();
        }

        /// <inheritdoc />
        public virtual Task<long> GetCountAsync(IQueryOptions<T> queryOptions)
        {
            var query = GetQuery(queryOptions);
            return query.LongCountAsync();
        }

        protected IQueryable<T> GetQuery(IQueryOptions<T> queryOptions)
        {
            IQueryable<T> query = EntitySet;

            if (queryOptions.Includes != null)
                query = queryOptions.Includes(query);

            if (queryOptions.OrderBy != null)
                query = queryOptions.OrderBy(query);

            if (queryOptions.Filter != null)
                query = query.Where(queryOptions.Filter);

            return query;
        }

        protected virtual void ReleaseManagedResources() => UnitOfWork?.Dispose();

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                ReleaseManagedResources();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
