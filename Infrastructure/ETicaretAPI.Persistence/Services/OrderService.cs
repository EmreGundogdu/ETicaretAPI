using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.Order;
using ETicaretAPI.Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Persistence.Services
{
    public class OrderService : IOrderService
    {
        readonly IOrderWriteRepository orderWriteRepository;
        readonly IOrderReadRepository orderReadRepository;
        readonly ICompletedOrderWriteRepository completedOrderWriteRepository;
        public OrderService(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository, ICompletedOrderWriteRepository completedOrderWriteRepository)
        {
            this.orderWriteRepository = orderWriteRepository;
            this.orderReadRepository = orderReadRepository;
            this.completedOrderWriteRepository = completedOrderWriteRepository;
        }

        public async Task CompleteOrderAsync(string id)
        {
            var order = await orderReadRepository.GetByIdAsync(id);
            if (order != null)
            {
                await completedOrderWriteRepository.AddAsync(new() { OrderId = Guid.Parse(id) });
                await completedOrderWriteRepository.SaveAsync();
            }

        }

        public async Task CreateOrder(CreateOrder createOrder)
        {
            var orderCode = (new Random().NextDouble() * 10000).ToString();
            orderCode = orderCode.Substring(orderCode.IndexOf(",") + 1, orderCode.Length - orderCode.IndexOf(",") - 1);
            await orderWriteRepository.AddAsync(new()
            {
                Address = createOrder.Address,
                Description = createOrder.Description,
                ID = Guid.Parse(createOrder.BasketId),
                OrderCode = orderCode
            });
            await orderWriteRepository.SaveAsync();
        }

        public async Task<ListOrder> GetAllOrdersAsync(int page, int pageSize)
        {
            var query = orderReadRepository.Table.Include(x => x.Basket).ThenInclude(x => x.User).Include(x => x.Basket).ThenInclude(x => x.BasketItems).ThenInclude(x => x.Product);
            var data = query.Skip(page * pageSize).Take(pageSize);
            return new()
            {
                TotalCount = await query.CountAsync(),
                Orders = await data.Select(x => new
                {
                    Id = x.ID,
                    CreatedDate = x.CreatedDate,
                    OrderCode = x.OrderCode,
                    TotalPrice = x.Basket.BasketItems.Sum(x => x.Product.Price * x.Quantity),
                    UserName = x.Basket.User.UserName
                }).ToListAsync()
            };
        }

        public async Task<Order> GetOrderByIdAsync(string id)
        {
            var data = await orderReadRepository.Table.Include(x => x.Basket).ThenInclude(x => x.BasketItems).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.ID == Guid.Parse(id));
            return new()
            {
                Id = data.ID.ToString(),
                BasketItems = data.Basket.BasketItems.Select(x => new
                {
                    x.Product.Name,
                    x.Product.Price,
                    x.Quantity
                }),
                Address = data.Address,
                CreatedDate = data.CreatedDate,
                Description = data.Description,
                OrderCode = data.OrderCode
            };
        }
    }
}
