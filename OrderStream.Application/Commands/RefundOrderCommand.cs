using MediatR;

namespace OrderStream.Application.Commands
{
    public class RefundOrderCommand : IRequest<bool>
    {
        public string OrderId { get; set; }
    }
}