namespace VendingMachine.Infrastructure.Core
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VendingMachine.Entities;

    public interface IReadRepository<T> : IDisposable where T : BaseEntity
    {
        DbSet<T> EntitySet { get; }
        Task<T?> GetEntityAsync(int id, IQueryOptions<T>? queryOptions = null);
        Task<T?> GetFirstOrDefaultAsync(IQueryOptions<T>? queryOptions = null);
        Task<IEnumerable<T>> GetListAsync(IQueryOptions<T>? queryOptions = null);
        Task<long> GetCountAsync(IQueryOptions<T>? queryOptions);
    }
}
