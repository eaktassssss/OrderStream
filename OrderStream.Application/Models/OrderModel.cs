
using OrderStream.Domain.Enums;

namespace OrderStream.Application.Models
{
    public class OrderModel
    {
        public string Id { get; set; }
        public int CustomerId { get; set; }
        public List<OrderItemModel> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public void CalculateTotal()
        {
            TotalAmount = Items.Sum(i => i.Price * i.Quantity);
        }
    }

    public class OrderItemModel
    {
        public string ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}