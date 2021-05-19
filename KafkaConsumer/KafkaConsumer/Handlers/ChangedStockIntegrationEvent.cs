using EventBus.Events;

namespace KafkaConsumer.Handlers
{
    public class ChangedStockIntegrationEvent : IntegrationEvent
    {
        public string Name { get; private set; }
        public int Price { get; private set; }

        public ChangedStockIntegrationEvent(string name, int price)
        {
            Name = name;
            Price = price;
        }
    }
}
