using MediatR;

namespace OrderStream.Application.Commands
{
    public class DiscontinueLowSellingProductsCommand : IRequest<bool>
    {
        public int SalesThreshold { get; set; }
    }
}