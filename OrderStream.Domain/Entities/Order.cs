using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OrderStream.Domain.Enums;

namespace OrderStream.Domain.Entities
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int CustomerId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }

        public void CalculateTotal()
        {
            TotalAmount = OrderItems.Sum(i => i.Price * i.Quantity);
        }
    }

    public class OrderItem
    {
        public string ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}