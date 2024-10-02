using MediatR;
using OrderStream.Application.Commands;
using OrderStream.Application.Models;
using OrderStream.Application.Services;

namespace OrderStream.Application.Handlers
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductService _productService;

        public UpdateProductHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new ProductModel
            {
                Id = request.Id,
                Name = request.Name,
                Price = request.Price,
                StockQuantity = request.StockQuantity
            };

            return _productService.UpdateProduct(product);
        }
    }
}