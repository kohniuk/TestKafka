using EventBus.Abstractions;
using KafkaConsumer.Handlers;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestKafka.Handlers
{
    public class KafkaConsumerHandler : IHostedService
    {
        private readonly IEventBus _eventBus;

        public KafkaConsumerHandler(IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _eventBus.Subscribe<ChangedStockIntegrationEvent, ChangedStockIntegrationEventHandler>();

            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}