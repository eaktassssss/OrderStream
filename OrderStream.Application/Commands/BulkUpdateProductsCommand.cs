using MediatR;
using OrderStream.Application.Models;

namespace OrderStream.Application.Commands
{
    public class BulkUpdateProductsCommand : IRequest<bool>
    {
        public List<ProductModel> Products { get; set; }
    }
}