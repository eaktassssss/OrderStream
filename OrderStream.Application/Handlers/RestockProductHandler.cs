using MediatR;
using OrderStream.Application.Commands;
using OrderStream.Application.Services;

namespace OrderStream.Application.Handlers
{
    public class RestockProductHandler : IRequestHandler<RestockProductCommand, bool>
    {
        private readonly IProductService _productService;

        public RestockProductHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<bool> Handle(RestockProductCommand request, CancellationToken cancellationToken)
        {
            return _productService.RestockProduct(request.ProductId, request.Quantity);
        }
    }
}