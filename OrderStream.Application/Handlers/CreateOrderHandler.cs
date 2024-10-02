using MediatR;
using OrderStream.Application.Commands;
using OrderStream.Application.Models;

using OrderStream.Application.Services;
using OrderStream.Domain.Enums;

namespace OrderStream.Application.Handlers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, bool>
    {
        private readonly IOrderService _orderService;

        public CreateOrderHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var newOrder = new OrderModel
            {
                CustomerId = request.CustomerId,
                Items = request.Items.Select(i => new OrderItemModel
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList(),
                TotalAmount = request.Items.Sum(i => i.Price * i.Quantity),
                OrderStatus = OrderStatus.Pending
            };

            _orderService.CreateOrder(newOrder);
            return true;
        }
    }
}