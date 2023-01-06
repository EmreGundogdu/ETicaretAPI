using ETicaretAPI.Application.Abstractions.Hubs;

namespace ETicaretAPI.SignalR.HubServices
{
    public class ProductHubService : IProductHubService
    {

        public Task ProductAddedMessageAsync(string message)
        {
            throw new NotImplementedException();
        }
    }
}
