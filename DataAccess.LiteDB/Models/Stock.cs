using System;

namespace DataAccess.LiteDB
{
    public class Stock
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }
}
