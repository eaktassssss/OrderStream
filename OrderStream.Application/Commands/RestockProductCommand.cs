using MediatR;

namespace OrderStream.Application.Commands
{
    public class RestockProductCommand : IRequest<bool>
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}