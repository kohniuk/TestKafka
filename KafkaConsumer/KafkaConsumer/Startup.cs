using DataAccess.LiteDB;
using EventBus;
using EventBus.Abstractions;
using EventBus.Kafka;
using KafkaConsumer.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using TestKafka.Handlers;

namespace KafkaConsumer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IKafkaPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultKafkaPersistentConnection>>();
                var clientConfig = new Dictionary<string, string>
                {
                    ["bootstrap.servers"] = "localhost:9092",
                    ["topic"] = "simpletalk_topic",
                    ["group.id"] = "st_consumer_group"
                };

                return new DefaultKafkaPersistentConnection(clientConfig, logger);
            });

            services.AddSingleton<IEventBus, EventBusKafka>(sp =>
            {
                var kafkaPersistentConnection = sp.GetRequiredService<IKafkaPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBusKafka>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                return new EventBusKafka(kafkaPersistentConnection, logger, eventBusSubcriptionsManager, sp);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            services.AddTransient<ChangedStockIntegrationEventHandler>();
            services.AddTransient<IStockService, StockService>();
            services.AddSingleton<IHostedService, KafkaConsumerHandler>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
