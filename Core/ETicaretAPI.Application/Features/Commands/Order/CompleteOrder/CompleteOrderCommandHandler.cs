using ETicaretAPI.Application.Abstractions.Services;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Order.CompleteOrder
{
    public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommandRequest, CompleteOrderCommandResponse>
    {
        readonly IOrderService orderService;
        readonly IMailService mailService;
        public CompleteOrderCommandHandler(IOrderService orderService, IMailService mailService)
        {
            this.orderService = orderService;
            this.mailService = mailService;
        }

        public async Task<CompleteOrderCommandResponse> Handle(CompleteOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var result =await orderService.CompleteOrderAsync(request.Id);
            if (result)
            {
                mailService.SendCompletedOrderMailAsync();
            }
            return new();
        }
    }
}
