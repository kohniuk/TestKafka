using DataAccess.LiteDB;
using EventBus.Abstractions;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace KafkaConsumer.Handlers
{
    public class ChangedStockIntegrationEventHandler : IIntegrationEventHandler<ChangedStockIntegrationEvent>
    {
        private readonly IStockService _stockService;
        public ChangedStockIntegrationEventHandler(IStockService stockService)
        {
            _stockService = stockService;
        }

        public async Task Handle(ChangedStockIntegrationEvent @event)
        {
            var logMessage = $"--- Subscribe -- {@event.Id} - message:{JsonConvert.SerializeObject(@event)}";

            var stock = new Stock()
            {
                Id = @event.Id,
                CreationDate = @event.CreationDate,
                Name = @event.Name,
                Price = @event.Price,
            };

            _stockService.Add(stock);
            Console.WriteLine(logMessage);

            await Task.CompletedTask;
        }
    }
}
