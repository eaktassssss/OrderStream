using MediatR;
using OrderStream.Application.Models;
using OrderStream.Application.Queries;
using OrderStream.Application.Services;

namespace OrderStream.Application.Handlers
{
    public class GetOrdersByCustomerHandler : IRequestHandler<GetOrdersByCustomerQuery, IEnumerable<OrderModel>>
    {
        private readonly IOrderService _orderService;

        public GetOrdersByCustomerHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IEnumerable<OrderModel>> Handle(GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
        {
            return _orderService.GetOrdersByCustomer(request.CustomerId);
        }
    }
}