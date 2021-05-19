using Hangfire;
using KafkaProducer.Events;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Diagnostics;

namespace KafkaProducer.Controllers
{
    [Route("api/kafka")]
    [ApiController]
    public class KafkaProducerController : ControllerBase
    {
        private static readonly string[] Stocks = new[]
        {
            "Apple", "Tesla", "Amazon", "Microsoft", "Facebook", "Google"
        };

        private readonly IChangedStockIntegrationEventService _stockIntegrationEventService;

        public KafkaProducerController(IChangedStockIntegrationEventService testIntegrationEventService)
        {
            _stockIntegrationEventService = testIntegrationEventService;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            RecurringJob.AddOrUpdate(() => Send(), "*/10 * * * * *");
            
            return Created(string.Empty, null);
        }

        public ChangedStockIntegrationEvent Send()
        {
            var rng = new Random();
            var stockName = Stocks[rng.Next(Stocks.Length)];
            var price = rng.Next(20, 150);

            var changedStockIntegrationEvent = new ChangedStockIntegrationEvent(stockName, price);

            _stockIntegrationEventService
                .PublishThroughEventBusAsync(changedStockIntegrationEvent)
                .GetAwaiter()
                .GetResult();

            Debug.WriteLine($"Here is your event, {changedStockIntegrationEvent.Name}, {changedStockIntegrationEvent.Price}");

            return changedStockIntegrationEvent;
        }
    }
}