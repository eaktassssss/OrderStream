using MediatR;
using OrderStream.Application.Models;

namespace OrderStream.Application.Queries
{
    public class GetPendingOrdersQuery : IRequest<IEnumerable<OrderModel>>
    {
    }
}