namespace VendingMachine.Core.Registry
{
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;
    using VendingMachine.Core.Behaviors;
    using VendingMachine.Core.Services;

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

            return services;
        }
    }
}
