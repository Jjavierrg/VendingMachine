namespace VendingMachine.Api
{
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;
    using VendingMachine.Api.Core;
    using VendingMachine.Api.Hubs;
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
            services.AddCors();
            services.AddSignalR();

            services.AddTransient<ExceptionValidationMiddleware>();

            AddDatabaseContext(services, Configuration);
            ConfigureCors(services);
        }

        public void Configure(IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            app.UseCors("CorsPolicy");
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseMiddleware<ExceptionValidationMiddleware>();

            ApplyMigrations(app);
        }
        private static void AddDatabaseContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<VendingUoW>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), option => option.MigrationsAssembly("VendingMachine.Infrastructure"));
                options.EnableSensitiveDataLogging();
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
        }

        private static void ApplyMigrations(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VendingUoW>();
            if (db.Database.GetPendingMigrations().Any())
                db.Database.Migrate();
        }

        private void ConfigureCors(IServiceCollection services)
        {
            var AllowedHosts = Configuration.GetSection("AllowedCorsOrigins").Value.Split(",");

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins(AllowedHosts)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
        }
    }
}
