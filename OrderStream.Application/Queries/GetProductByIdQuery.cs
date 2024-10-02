using MediatR;
using OrderStream.Application.Models;

namespace OrderStream.Application.Queries
{
    public class GetProductByIdQuery : IRequest<ProductModel>
    {
        public string ProductId { get; set; }
    }
}