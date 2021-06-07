using Hangfire;
using Hangfire.SqlServer;
using MassTransit;
using masstransit_api.EventConsumer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace masstransit_api.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("HangfireConnection");

            JobStorage.Current = new SqlServerStorage(connectionString);

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            // Add MassTransit services.
            services.AddMassTransit(x =>
            {
                x.AddMessageScheduler(new Uri("queue:scheduler"));

                x.AddConsumer<ValueEnteredEventConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseHangfireScheduler("scheduler");
                    cfg.UseMessageScheduler(new Uri("queue:scheduler"));
                    cfg.ConfigureEndpoints(context);
                });
            });

            // Add the processing server as IHostedService
            services.AddMassTransitHostedService();

            return services;
        }
    }
}
