using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Order.CompleteOrder
{
    public class CompleteOrderCommandRequest:IRequest<CompleteOrderCommandResponse>
    {
    }
}