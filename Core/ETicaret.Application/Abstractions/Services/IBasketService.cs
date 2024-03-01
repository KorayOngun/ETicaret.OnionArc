using ETicaret.Application.ViewModels.Baskets;
using ETicaret.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Application.Abstractions.Services
{
    public interface IBasketService
    {
        Task<List<BasketItem>> GetBasketItemsAsync();
        Task AddItemToBasketAsync(VM_Create_BasketItem basketItem);
        Task UpdateQuantityAsync(VM_Update_BasketItem basketItem);
        Task RemoveBasketItemAsync(string basketItemId);
        Basket? GetUserBasket { get; } 
    }
}
