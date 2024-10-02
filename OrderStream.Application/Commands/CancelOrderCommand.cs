using MediatR;

namespace OrderStream.Application.Commands
{
    public class CancelOrderCommand : IRequest<bool>
    {
        public string OrderId { get; set; }
    }
}