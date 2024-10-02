using MediatR;

namespace OrderStream.Application.Commands
{
    public class CompleteOrderCommand : IRequest<bool>
    {
        public string OrderId { get; set; }
    }
}