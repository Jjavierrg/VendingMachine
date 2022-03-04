namespace VendingMachine.Infrastructure.Core
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VendingMachine.Entities;

    public interface IRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        Task SaveChangesAsync();
        void Add(T item);
        void AddRange(IEnumerable<T> items);
        void Update(T item);
        void Remove(T item);
        void RemoveRange(IEnumerable<T> items);
    }
}
