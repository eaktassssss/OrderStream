using MediatR;
using OrderStream.Application.Models;

namespace OrderStream.Application.Queries
{
    public class GetOrderQuery : IRequest<OrderModel>
    {
        public string OrderId { get; set; }
    }
}