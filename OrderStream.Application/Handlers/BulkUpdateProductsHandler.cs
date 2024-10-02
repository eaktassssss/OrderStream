using MediatR;
using OrderStream.Application.Commands;
using OrderStream.Application.Services;

namespace OrderStream.Application.Handlers
{
    public class BulkUpdateProductsHandler : IRequestHandler<BulkUpdateProductsCommand, bool>
    {
        private readonly IProductService _productService;

        public BulkUpdateProductsHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<bool> Handle(BulkUpdateProductsCommand request, CancellationToken cancellationToken)
        {
            return _productService.BulkUpdateProducts(request.Products);
        }
    }
}