using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrderStream.Domain.Entities
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public bool IsArchived { get; set; }
        public int SalesCount { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsDiscontinued { get; set; }
    }
}