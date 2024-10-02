using MediatR;
using OrderStream.Application.Models;

namespace OrderStream.Application.Commands
{
    public class PartialShipOrderCommand : IRequest<bool>
    {
        public string OrderId { get; set; }
        public List<OrderItemModel> ShippedItems { get; set; }
    }
}