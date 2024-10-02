using MediatR;
using OrderStream.Application.Commands;
using OrderStream.Application.Services;

namespace OrderStream.Application.Handlers
{
    public class ArchiveProductHandler : IRequestHandler<ArchiveProductCommand, bool>
    {
        private readonly IProductService _productService;

        public ArchiveProductHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<bool> Handle(ArchiveProductCommand request, CancellationToken cancellationToken)
        {
            return _productService.ArchiveProduct(request.ProductId);
        }
    }
}