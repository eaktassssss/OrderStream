using MediatR;
using OrderStream.Application.Models;
using OrderStream.Application.Queries;
using OrderStream.Application.Services;

namespace OrderStream.Application.Handlers
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductModel>>
    {
        private readonly IProductService _productService;

        public GetAllProductsHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IEnumerable<ProductModel>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return _productService.GetAllProducts();
        }
    }
}