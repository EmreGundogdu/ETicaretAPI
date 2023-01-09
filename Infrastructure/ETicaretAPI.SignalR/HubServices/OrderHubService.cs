using ETicaretAPI.Application.Abstractions.Hubs;
using ETicaretAPI.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ETicaretAPI.SignalR.HubServices
{
    public class OrderHubService : IOrderHubService
    {
        readonly IHubContext<OrderHub> hubContext;

        public OrderHubService(IHubContext<OrderHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public async Task OrderAddedMessageAsync(string message)
           => await hubContext.Clients.All.SendAsync(ReceiveFunctionNames.OrderAddedMessage, message);
    }
}
