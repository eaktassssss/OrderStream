using MediatR;

namespace OrderStream.Application.Commands
{
    public class AdjustProductPricingBasedOnStockCommand : IRequest<bool>
    {
        public string ProductId { get; set; }
        public int LowStockThreshold { get; set; }
        public decimal PriceIncreasePercentage { get; set; }
        public int HighStockThreshold { get; set; }
        public decimal PriceDecreasePercentage { get; set; }
    }
}