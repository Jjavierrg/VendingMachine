namespace VendingMachine.Domain.Registry
{
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;
    using VendingMachine.Domain.Behaviors;
    using VendingMachine.Domain.Services;

    public static class Registry
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddMediatR(assembly);
            services.AddValidatorsFromAssembly(assembly);
            services.AddAutoMapper(assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IChangeService, ChangeService>();
            services.AddScoped<ISaleService, SellService>();

            return services;
        }
    }
}
