using ETicaretAPI.Application.Abstractions.Hubs;
using ETicaretAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.Order.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommandRequest, CreateOrderCommandResponse>
    {
        readonly IOrderService orderService;
        readonly IBasketService basketService;
        readonly IOrderHubService orderHubService;
        public CreateOrderCommandHandler(IOrderService orderService, IBasketService basketService, IOrderHubService orderHubService)
        {
            this.orderService = orderService;
            this.basketService = basketService;
            this.orderHubService = orderHubService;
        }

        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
        {
            await orderService.CreateOrder(new()
            {
                Address = request.Address,
                Description = request.Description,
                BasketId = basketService.GetUserActiveBasketAsync?.ID.ToString()
            });
            await orderHubService.OrderAddedMessageAsync("Yeni bir sipariş geldi");
            return new();
        }
    }
}
