using MediatR;
using OrderStream.Application.Commands;
using OrderStream.Application.Services;

namespace OrderStream.Application.Handlers
{
    public class ProcessOrderInStagesHandler : IRequestHandler<ProcessOrderInStagesCommand, bool>
    {
        private readonly IOrderService _orderService;

        public ProcessOrderInStagesHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<bool> Handle(ProcessOrderInStagesCommand request, CancellationToken cancellationToken)
        {
            return _orderService.ProcessOrderInStages(request.OrderId, request.Stages);
        }
    }
}