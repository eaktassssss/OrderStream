using MediatR;
using OrderStream.Application.Commands;
using OrderStream.Application.Services;

namespace OrderStream.Application.Handlers
{
    public class DiscountProductHandler : IRequestHandler<DiscountProductCommand, bool>
    {
        private readonly IProductService _productService;

        public DiscountProductHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<bool> Handle(DiscountProductCommand request, CancellationToken cancellationToken)
        {
            return _productService.DiscountProduct(request.ProductId, request.DiscountPercentage);
        }
    }
}