using MediatR;
using OrderStream.Domain.Enums;

 

namespace OrderStream.Application.Commands
{
    public class ProcessOrderInStagesCommand : IRequest<bool>
    {
        public string OrderId { get; set; }
        public List<OrderStatus> Stages { get; set; }
    }
}