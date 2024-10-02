using MediatR;

namespace OrderStream.Application.Commands
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public string ProductId { get; set; }
    }
}