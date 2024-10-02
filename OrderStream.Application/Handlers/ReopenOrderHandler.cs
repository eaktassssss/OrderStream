using MediatR;
using OrderStream.Application.Commands;
using OrderStream.Application.Services;

namespace OrderStream.Application.Handlers
{
    public class ReopenOrderHandler : IRequestHandler<ReopenOrderCommand, bool>
    {
        private readonly IOrderService _orderService;

        public ReopenOrderHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<bool> Handle(ReopenOrderCommand request, CancellationToken cancellationToken)
        {
            return _orderService.ReopenOrder(request.OrderId);
        }
    }
}