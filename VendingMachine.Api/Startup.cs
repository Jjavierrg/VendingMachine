namespace VendingMachine.Api
{
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;
    using VendingMachine.Api.Core;
    using VendingMachine.Core.Registry;
    using VendingMachine.Infrastructure;

    public class Startup
    {
        public Startup(IConfigurationRoot configuration)
        {
            Configuration = configuration;
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddInfrastructure();
            services.AddCore();

            services.AddTransient<ExceptionValidationMiddleware>();

            AddDatabaseContext(services, Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseMiddleware<ExceptionValidationMiddleware>();

            ApplyMigrations(app);
        }
        private static void AddDatabaseContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSqlServer<VendingUoW>(configuration.GetConnectionString("DefaultConnection"), (options) =>
            {
                options.MigrationsAssembly("VendingMachine.Infrastructure");
            });
        }

        private static void ApplyMigrations(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VendingUoW>();
            if (db.Database.GetPendingMigrations().Any())
                db.Database.Migrate();
        }
    }
}
