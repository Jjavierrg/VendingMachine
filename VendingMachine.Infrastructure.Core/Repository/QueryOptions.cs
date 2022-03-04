namespace VendingMachine.Infrastructure.Core
{
    using Microsoft.EntityFrameworkCore.Query;
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using VendingMachine.Entities;

    public interface IQueryOptions<T> where T : BaseEntity
    {
        Expression<Func<T, bool>>? Filter { get; set; }
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? Includes { get; set; }
        Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; set; }
    }

    public class QueryOptions<T> : IQueryOptions<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>>? Filter { get; set; }
        public Func<IQueryable<T>, IIncludableQueryable<T, object>>? Includes { get; set; }
        public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; set; }
    }
}
