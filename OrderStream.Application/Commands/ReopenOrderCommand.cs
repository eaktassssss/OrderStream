using MediatR;

namespace OrderStream.Application.Commands
{
    public class ReopenOrderCommand : IRequest<bool>
    {
        public string OrderId { get; set; }
    }
}