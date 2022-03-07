namespace VendingMachine.Infrastructure
{
    using Microsoft.Extensions.DependencyInjection;
    using VendingMachine.Infrastructure.Core;

    public static class Registry
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, VendingUoW>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient(typeof(IReadRepository<>), typeof(ReadRepository<>));

            services.AddTransient(typeof(IQueryOptions<>), typeof(QueryOptions<>));

            return services;
        }
    }
}
