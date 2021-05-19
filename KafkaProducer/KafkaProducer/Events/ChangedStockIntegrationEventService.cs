using EventBus.Abstractions;
using EventBus.Events;
using System;
using System.Threading.Tasks;

namespace KafkaProducer.Events
{
    public class ChangedStockIntegrationEventService : IChangedStockIntegrationEventService
    {
        private readonly IEventBus _eventBus;

        public ChangedStockIntegrationEventService(IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task PublishThroughEventBusAsync(IntegrationEvent @event)
        {
            _eventBus.Publish(@event);

            await Task.FromResult(true);
        }
    }
}
