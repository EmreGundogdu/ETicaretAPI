using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.Order;
using ETicaretAPI.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Services
{
    public class OrderService : IOrderService
    {
        readonly IOrderWriteRepository orderWriteRepository;

        public OrderService(IOrderWriteRepository orderWriteRepository)
        {
            this.orderWriteRepository = orderWriteRepository;
        }

        public async Task CreateOrder(CreateOrder createOrder)
        {
            await orderWriteRepository.AddAsync(new()
            {
                Address = createOrder.Address,
                Description = createOrder.Description,
                ID = Guid.Parse(createOrder.BasketId)
            });
            await orderWriteRepository.SaveAsync();
        }
    }
}
