using LiteDB;
using System;
using System.Collections.Generic;

namespace DataAccess.LiteDB
{
    public class StockService : IStockService
    {
        private readonly string dbPath = @"C:\tmp\stoks.db";

        public Guid Add(Stock stock)
        {
            using (var db = new LiteDatabase(dbPath))
            {
                var repository = db.GetCollection<Stock>("stoks");

                if (repository.FindById(stock.Id) != null)
                    repository.Update(stock);
                else
                    repository.Insert(stock);

                return stock.Id;
            }
        }

        public ICollection<Stock> GetAll()
        {
            using (var db = new LiteDatabase(dbPath))
            {
                var repository = db.GetCollection<Stock>("stoks");

                return repository
                    .Query()
                    .ToList();
            }
        }
    }
}
