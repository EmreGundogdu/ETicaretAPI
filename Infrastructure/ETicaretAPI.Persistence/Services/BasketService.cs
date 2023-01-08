using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.ViewModels.Baskets;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Persistence.Services
{
    public class BasketService : IBasketService
    {
        readonly IHttpContextAccessor httpContextAccessor;
        readonly UserManager<AppUser> userManager;
        readonly IOrderReadRepository orderReadRepository;
        readonly IBasketWriterRepository basketWriterRepository;
        readonly IBasketItemWriteRepository basketItemWriteRepository;
        readonly IBasketItemReadRepository basketItemReadRepository;
        readonly IBasketReadRepository basketReadRepository;
        public BasketService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IOrderReadRepository orderReadRepository, IBasketWriterRepository basketWriterRepository, IBasketItemWriteRepository basketItemWriteRepository, IBasketItemReadRepository basketItemReadRepository, IBasketReadRepository basketReadRepository)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            this.orderReadRepository = orderReadRepository;
            this.basketWriterRepository = basketWriterRepository;
            this.basketItemWriteRepository = basketItemWriteRepository;
            this.basketItemReadRepository = basketItemReadRepository;
            this.basketReadRepository = basketReadRepository;
        }
        private async Task<Basket?> ContextUser()
        {
            var username = httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            if (!string.IsNullOrEmpty(username))
            {
                AppUser? appUser = await userManager.Users.Include(x => x.Baskets).FirstOrDefaultAsync(x => x.UserName == username);
                var _basket = from basket in appUser.Baskets
                              join order in orderReadRepository.Table on basket.ID equals order.ID into BasketOrders
                              from order in BasketOrders.DefaultIfEmpty()
                              select new
                              {
                                  Basket = basket,
                                  Order = order
                              };
                Basket? targetBasket = null;

                if (_basket.Any(x => x.Order is null))
                    targetBasket = _basket.FirstOrDefault(x => x.Order is null)?.Basket;
                else
                {
                    targetBasket = new();
                    appUser.Baskets.Add(targetBasket);
                }
                await basketWriterRepository.SaveAsync();
                return targetBasket;
            }
            throw new Exception("Beklenmeyen bir hata ile karşılaşıldı.");
        }

        public async Task AddItemToBasketAsync(VM_Create_BasketItem basketItem)
        {
            Basket? basket = await ContextUser();
            if (basket != null)
            {
                BasketItem _basketItem = await basketItemReadRepository.GetSingleAsync(x => x.BasketId == basket.ID && x.ProductId == Guid.Parse(basketItem.ProductId));
                if (_basketItem != null)
                {
                    _basketItem.Quantity++;
                }
                else
                {
                    await basketItemWriteRepository.AddAsync(new()
                    {
                        BasketId = basket.ID,
                        ProductId = Guid.Parse(basketItem.ProductId),
                        Quantity = basketItem.Quantity
                    });
                    await basketItemWriteRepository.SaveAsync();
                }
            }
        }

        public async Task<List<BasketItem>> GetBasketItems()
        {
            Basket basket = await ContextUser();
            Basket result = await basketReadRepository.Table.Include(x => x.BasketItems).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.ID == basket.ID);
            return result.BasketItems.ToList();
        }

        public async Task RemoveBasketItemAsync(string basketItemId)
        {
            BasketItem basketItem = await basketItemReadRepository.GetByIdAsync(basketItemId);
            if (basketItem != null)
            {
                basketItemWriteRepository.Remove(basketItem);
                await basketItemWriteRepository.SaveAsync();
            }
        }

        public async Task UpdateQuantityAsync(VM_Update_BasketItem basketItem)
        {
            BasketItem _basketItem = await basketItemReadRepository.GetByIdAsync(basketItem.BasketItemId);
            if (_basketItem!=null)
            {
                _basketItem.Quantity = basketItem.Quantity;
                await basketItemWriteRepository.SaveAsync();
            }
        }
    }
}
