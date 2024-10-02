using MediatR;
using OrderStream.Application.Commands;
using OrderStream.Application.Services;

namespace OrderStream.Application.Handlers
{
    public class CompleteOrderHandler : IRequestHandler<CompleteOrderCommand, bool>
    {
        private readonly IOrderService _orderService;

        public CompleteOrderHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<bool> Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
        {
            return _orderService.CompleteOrder(request.OrderId);
        }
    }
}