namespace VendingMachine.Domain.Tests.Shared
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using VendingMachine.Entities;
    using VendingMachine.Infrastructure.Core;

    public class RepositoryMoq<T> : IRepository<T>, IReadRepository<T> where T : BaseEntity
    {
        public DbSet<T> EntitySet => throw new System.NotImplementedException();
        private readonly List<T> _entities = new();

        public RepositoryMoq(): this(new List<T>())
        {
        }

        public RepositoryMoq(IEnumerable<T> entities)
        {
            _entities = entities.ToList();
        }

        public void Add(T item)
        {
            if (item == null)
                return;

            var maxId = _entities.Any() ? _entities.Max(x => x.Id) : 0;
            item.Id = maxId + 1;
            _entities.Add(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            _entities.AddRange(items);
        }

        public void Dispose()
        {
        }

        public Task<long> GetCountAsync(IQueryOptions<T>? queryOptions = null)
        {
            return Task.FromResult(_entities.LongCount());
        }

        public Task<T?> GetEntityAsync(int id, IQueryOptions<T>? queryOptions = null)
        {
            var result = _entities.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(result);
        }

        public Task<T?> GetFirstOrDefaultAsync(IQueryOptions<T>? queryOptions = null)
        {
            return Task.FromResult(_entities.FirstOrDefault());
        }

        public Task<IEnumerable<T>> GetListAsync(IQueryOptions<T>? queryOptions = null)
        {
            return Task.FromResult(_entities.AsEnumerable());
        }

        public void Remove(T item)
        {
            _entities.Remove(item);
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            items.ToList().ForEach(x => _entities.Remove(x));
        }

        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
        }

        public void Update(T item)
        {
            var index = _entities.FindIndex(x => x.Id == item.Id);
            if (index >= 0)
                _entities[index] = item;
        }
    }
}
