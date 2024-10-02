using MediatR;
using OrderStream.Application.Commands;
using OrderStream.Application.Services;

namespace OrderStream.Application.Handlers
{
    public class AdjustProductPricingBasedOnStockHandler : IRequestHandler<AdjustProductPricingBasedOnStockCommand, bool>
    {
        private readonly IOrderService _orderService;

        public AdjustProductPricingBasedOnStockHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<bool> Handle(AdjustProductPricingBasedOnStockCommand request, CancellationToken cancellationToken)
        {
            return _orderService.AdjustProductPricingBasedOnStock(request.ProductId, request.LowStockThreshold, request.PriceIncreasePercentage, request.HighStockThreshold, request.PriceDecreasePercentage);
        }
    }
}