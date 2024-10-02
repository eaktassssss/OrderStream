using MediatR;
using OrderStream.Application.Models;
using OrderStream.Application.Queries;
using OrderStream.Application.Services;

namespace OrderStream.Application.Handlers
{
    public class GetOutOfStockProductsHandler : IRequestHandler<GetOutOfStockProductsQuery, IEnumerable<ProductModel>>
    {
        private readonly IProductService _productService;

        public GetOutOfStockProductsHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IEnumerable<ProductModel>> Handle(GetOutOfStockProductsQuery request, CancellationToken cancellationToken)
        {
            return _productService.GetOutOfStockProducts();
        }
    }
}