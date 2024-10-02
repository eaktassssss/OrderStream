using MediatR;
using OrderStream.Application.Commands;
using OrderStream.Application.Services;

namespace OrderStream.Application.Handlers
{
    public class ReturnOrderHandler : IRequestHandler<ReturnOrderCommand, bool>
    {
        private readonly IOrderService _orderService;

        public ReturnOrderHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<bool> Handle(ReturnOrderCommand request, CancellationToken cancellationToken)
        {
            return _orderService.ReturnOrder(request.OrderId, request.ReturnedItems);
        }
    }
}