using System;
using System.Collections.Generic;

namespace DataAccess.LiteDB
{
    public interface IStockService
    {
        Guid Add(Stock stock);

        ICollection<Stock> GetAll();
    }
}
