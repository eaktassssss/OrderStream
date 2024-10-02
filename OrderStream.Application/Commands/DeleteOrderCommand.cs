using MediatR;

namespace OrderStream.Application.Commands
{
    public class DeleteOrderCommand : IRequest<bool>
    {
        public string OrderId { get; set; }
    }
}