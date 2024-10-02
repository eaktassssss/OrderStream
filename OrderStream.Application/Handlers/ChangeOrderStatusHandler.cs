using MediatR;
using OrderStream.Application.Commands;
using OrderStream.Application.Services;

namespace OrderStream.Application.Handlers
{
    public class ChangeOrderStatusHandler : IRequestHandler<ChangeOrderStatusCommand, bool>
    {
        private readonly IOrderService _orderService;

        public ChangeOrderStatusHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<bool> Handle(ChangeOrderStatusCommand request, CancellationToken cancellationToken)
        {
            return _orderService.ChangeOrderStatus(request.OrderId, request.NewStatus);
        }
    }
}