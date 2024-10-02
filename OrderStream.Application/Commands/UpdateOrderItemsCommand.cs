using MediatR;
using OrderStream.Application.Models;

namespace OrderStream.Application.Commands
{
    public class UpdateOrderItemsCommand : IRequest<bool>
    {
        public string OrderId { get; set; }
        public List<OrderItemModel> UpdatedItems { get; set; }
    }
}