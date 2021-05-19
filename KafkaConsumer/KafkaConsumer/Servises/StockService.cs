using KafkaConsumer.Models;
using LiteDB;
using System;

namespace KafkaConsumer.Servises
{
    public class StockService : IStockService
    {
        public Guid Add(Stock stock)
        {
            using (var db = new LiteDatabase(@"C:\tmp\stoks.db"))
            {
                var repository = db.GetCollection<Stock>("stoks");

                if (repository.FindById(stock.Id) != null)
                    repository.Update(stock);
                else
                    repository.Insert(stock);

                return stock.Id;
            }
        }
    }
}
