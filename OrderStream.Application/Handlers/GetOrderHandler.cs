using MediatR;
using OrderStream.Application.Models;
using OrderStream.Application.Queries;
using OrderStream.Application.Services;

namespace OrderStream.Application.Handlers
{
    public class GetOrderHandler : IRequestHandler<GetOrderQuery, OrderModel>
    {
        private readonly IOrderService _orderService;

        public GetOrderHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<OrderModel> Handle(GetOrderQuery query, CancellationToken cancellationToken)
        {
            return _orderService.GetOrderById(query.OrderId);
        }
    }
}