using MediatR;
using OrderStream.Application.Models;

namespace OrderStream.Application.Queries
{
    public class GetOrdersByCustomerQuery : IRequest<IEnumerable<OrderModel>>
    {
        public int CustomerId { get; set; }
    }
}