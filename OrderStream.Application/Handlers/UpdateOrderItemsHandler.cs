using MediatR;
using OrderStream.Application.Commands;
using OrderStream.Application.Services;

namespace OrderStream.Application.Handlers
{
    public class UpdateOrderItemsHandler : IRequestHandler<UpdateOrderItemsCommand, bool>
    {
        private readonly IOrderService _orderService;

        public UpdateOrderItemsHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<bool> Handle(UpdateOrderItemsCommand request, CancellationToken cancellationToken)
        {
            return _orderService.UpdateOrderItems(request.OrderId, request.UpdatedItems);
        }
    }
}