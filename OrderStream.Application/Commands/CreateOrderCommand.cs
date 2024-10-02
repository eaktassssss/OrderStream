using MediatR;
using OrderStream.Application.Models;

namespace OrderStream.Application.Commands
{
    public class CreateOrderCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public int CustomerId { get; set; }
        public List<OrderItemModel> Items { get; set; }
    }
}