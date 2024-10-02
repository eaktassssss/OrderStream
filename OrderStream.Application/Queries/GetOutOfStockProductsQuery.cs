using MediatR;
using OrderStream.Application.Models;

namespace OrderStream.Application.Queries
{
    public class GetOutOfStockProductsQuery : IRequest<IEnumerable<ProductModel>>
    {
    }
}