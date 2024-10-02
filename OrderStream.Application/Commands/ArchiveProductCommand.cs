using MediatR;

namespace OrderStream.Application.Commands
{
    public class ArchiveProductCommand : IRequest<bool>
    {
        public string ProductId { get; set; }
    }
}