using MediatR;
using OrderStream.Application.Models;

namespace OrderStream.Application.Queries
{
    public class GetAllOrdersQuery : IRequest<IEnumerable<OrderModel>>
    {
    }
}