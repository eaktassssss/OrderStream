using MediatR;
using OrderStream.Application.Models;
using OrderStream.Application.Queries;
using OrderStream.Application.Services;

namespace OrderStream.Application.Handlers
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderModel>
    {
        private readonly IOrderService _orderService;

        public GetOrderByIdHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<OrderModel> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = _orderService.GetOrderById(request.OrderId);
            if (order == null) return null;

            return new OrderModel
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                Items = order.Items,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus
            };
        }
    }
}