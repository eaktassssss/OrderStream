using MediatR;
using OrderStream.Domain.Enums;

namespace OrderStream.Application.Commands
{
    public class ChangeOrderStatusCommand : IRequest<bool>
    {
        public string OrderId { get; set; }
        public OrderStatus NewStatus { get; set; }
    }
}