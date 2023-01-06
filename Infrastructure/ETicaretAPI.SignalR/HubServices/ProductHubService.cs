using ETicaretAPI.Application.Abstractions.Hubs;
using ETicaretAPI.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ETicaretAPI.SignalR.HubServices
{
    public class ProductHubService : IProductHubService
    {
        readonly IHubContext<ProductHub> hubContext;

        public ProductHubService(IHubContext<ProductHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public async Task ProductAddedMessageAsync(string message)
        {
            await hubContext.Clients.All.SendAsync(ReceiveFunctionNames.ProductAddedMessage, message);
        }
    }
}
