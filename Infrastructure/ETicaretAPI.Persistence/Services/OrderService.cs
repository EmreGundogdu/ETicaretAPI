using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.Order;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Persistence.Services
{
    public class OrderService : IOrderService
    {
        readonly IOrderWriteRepository orderWriteRepository;
        readonly IOrderReadRepository orderReadRepository;
        readonly ICompletedOrderWriteRepository completedOrderWriteRepository;
        readonly ICompletedOrderReadRepository completedOrderReadRepository;
        public OrderService(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository, ICompletedOrderWriteRepository completedOrderWriteRepository, ICompletedOrderReadRepository completedOrderReadRepository)
        {
            this.orderWriteRepository = orderWriteRepository;
            this.orderReadRepository = orderReadRepository;
            this.completedOrderWriteRepository = completedOrderWriteRepository;
            this.completedOrderReadRepository = completedOrderReadRepository;
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

            var data2 = from order in data join completedOrder in completedOrderReadRepository.Table on order.ID equals completedOrder.OrderId into co from _co in co.DefaultIfEmpty() select new { Id = order.ID, CreatedDate = order.CreatedDate, OrderCode = order.OrderCode, Basket = order.Basket, Completed = _co != null ? true : false };

            return new()
            {
                TotalCount = await query.CountAsync(),
                Orders = await data2.Select(x => new
                {
                    Id = x.Id,
                    CreatedDate = x.CreatedDate,
                    OrderCode = x.OrderCode,
                    TotalPrice = x.Basket.BasketItems.Sum(x => x.Product.Price * x.Quantity),
                    UserName = x.Basket.User.UserName,
                    x.Completed
                }).ToListAsync()
            };
        }

        public async Task<Application.DTOs.Order.Order> GetOrderByIdAsync(string id)
        {
            var data = orderReadRepository.Table.Include(x => x.Basket).ThenInclude(x => x.BasketItems).ThenInclude(x => x.Product);
            var data2 = await(from order in data
                              join completedOrder in completedOrderReadRepository.Table on order.ID equals completedOrder.OrderId into co
                              from _co in co.DefaultIfEmpty()
                              select new
                              {
                                  Id = order.ID,
                                  CreatedDate = order.CreatedDate,
                                  OrderCode = order.OrderCode,
                                  Basket = order.Basket,
                                  Completed = _co != null ? true : false,
                                  Address = order.Address,
                                  Description = order.Description
                              }).FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
            return new()
            {
                Id = data2.Id.ToString(),
                BasketItems = data2.Basket.BasketItems.Select(x => new
                {
                    x.Product.Name,
                    x.Product.Price,
                    x.Quantity
                }),
                Address = data2.Address,
                CreatedDate = data2.CreatedDate,
                Description = data2.Description,
                OrderCode = data2.OrderCode,
                Completed = data2.Completed
            };
        }
    }
}
