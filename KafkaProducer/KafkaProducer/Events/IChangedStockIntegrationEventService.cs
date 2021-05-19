using EventBus.Events;
using System.Threading.Tasks;

namespace KafkaProducer.Events
{
    public interface IChangedStockIntegrationEventService
    {
        Task PublishThroughEventBusAsync(IntegrationEvent @event);
    }
}
