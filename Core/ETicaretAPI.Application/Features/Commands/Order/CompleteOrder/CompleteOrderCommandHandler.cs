using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.Order;
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
            //(bool succeeded, CompletedOrder dto) result = await orderService.CompleteOrderAsync(request.Id);
            (bool succeeded, CompletedOrder dto) = await orderService.CompleteOrderAsync(request.Id);
            if (succeeded)
            {
                //mailService.SendCompletedOrderMailAsync(result.dto.Email,result.dto.OrderCode,result.dto.OrderDate,);
                await mailService.SendCompletedOrderMailAsync(dto.Email,dto.OrderCode,dto.OrderDate,dto.Username);
            }
            return new();
        }
    }
}
