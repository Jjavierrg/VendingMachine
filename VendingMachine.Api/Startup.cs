﻿namespace VendingMachine.Api
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
            services.AddCors();

            services.AddTransient<ExceptionValidationMiddleware>();

            AddDatabaseContext(services, Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseMiddleware<ExceptionValidationMiddleware>();

            app.UseCors(builder =>
            {
                builder.AllowAnyHeader()
                .AllowAnyOrigin()
                .AllowAnyMethod();
            });

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
    }
}
