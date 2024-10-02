using MediatR;
using OrderStream.Application.Models;

namespace OrderStream.Application.Commands
{
    public class ReturnOrderCommand : IRequest<bool>
    {
        public string OrderId { get; set; }
        public List<OrderItemModel> ReturnedItems { get; set; }
    }
}