using MediatR;
using OrderStream.Application.Models;
using OrderStream.Application.Queries;
using OrderStream.Application.Services;

namespace OrderStream.Application.Handlers
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductModel>
    {
        private readonly IProductService _productService;

        public GetProductByIdHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ProductModel> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = _productService.GetProductById(request.ProductId);

            if (product != null)
                return product;

            return null;
        }
    }
}