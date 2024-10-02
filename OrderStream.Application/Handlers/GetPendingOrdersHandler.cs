using MediatR;
using OrderStream.Application.Models;
using OrderStream.Application.Queries;
using OrderStream.Application.Services;

namespace OrderStream.Application.Handlers
{
    public class GetPendingOrdersHandler : IRequestHandler<GetPendingOrdersQuery, IEnumerable<OrderModel>>
    {
        private readonly IOrderService _orderService;

        public GetPendingOrdersHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IEnumerable<OrderModel>> Handle(GetPendingOrdersQuery request, CancellationToken cancellationToken)
        {
            return _orderService.GetPendingOrders();
        }
    }
}