using MediatR;
using OrderStream.Application.Commands;
using OrderStream.Application.Services;

namespace OrderStream.Application.Handlers
{
    public class RefundOrderHandler : IRequestHandler<RefundOrderCommand, bool>
    {
        private readonly IOrderService _orderService;

        public RefundOrderHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<bool> Handle(RefundOrderCommand request, CancellationToken cancellationToken)
        {
            return _orderService.RefundOrder(request.OrderId);
        }
    }
}