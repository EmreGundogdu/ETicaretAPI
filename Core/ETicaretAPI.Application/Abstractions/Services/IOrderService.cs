using ETicaretAPI.Application.DTOs.Order;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task CreateOrder(CreateOrder createOrder);
        Task<ListOrder> GetAllOrdersAsync(int page, int pageSize);
        Task<Order> GetOrderByIdAsync(string id);
        Task<(bool,CompletedOrder)> CompleteOrderAsync(string id);
    }
}
