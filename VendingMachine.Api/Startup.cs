namespace VendingMachine.Api
{
    using Microsoft.EntityFrameworkCore;
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

            AddDatabaseContext(services, Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

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
            var db = app.ApplicationServices.GetRequiredService<VendingUoW>();
            if (db.Database.GetPendingMigrations().Any())
                db.Database.Migrate();
        }
    }
}
