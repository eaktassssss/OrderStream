using MediatR;
using OrderStream.Application.Models;

namespace OrderStream.Application.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductModel>>
    {
    }
}