using KafkaConsumer.Models;
using System;

namespace KafkaConsumer.Servises
{
    public interface IStockService
    {
        Guid Add(Stock stock);
    }
}
