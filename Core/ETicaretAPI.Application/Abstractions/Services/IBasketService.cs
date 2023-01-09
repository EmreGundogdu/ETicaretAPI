using ETicaretAPI.Application.ViewModels.Baskets;
using ETicaretAPI.Domain.Entities;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IBasketService
    {
        Task<List<BasketItem>> GetBasketItems();
        Task AddItemToBasketAsync(VM_Create_BasketItem vM_Create_BasketItem);
        Task UpdateQuantityAsync(VM_Update_BasketItem vM_Update_BasketItem);
        Task RemoveBasketItemAsync(string basketItemId);

    }
}
