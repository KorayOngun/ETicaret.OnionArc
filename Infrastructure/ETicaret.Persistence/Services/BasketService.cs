using ETicaret.Application.Abstractions.Services;
using ETicaret.Application.Repositories;
using ETicaret.Application.ViewModels.Baskets;
using ETicaret.Domain.Entities;
using ETicaret.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Persistence.Services
{
    public class BasketService : IBasketService
    {
        readonly IHttpContextAccessor _contextAccessor;
        readonly UserManager<AppUser> _userManager;
        readonly IOrderReadRepository _orderReadRepo;
        readonly IBasketWriteRepository _basketWriteRepository;
        readonly IBasketReadRepository _basketReadRepository;
        readonly IBasketItemWriteRepository _basketItemWriteRepository;
        readonly IBasketItemReadRepository _basketItemReadRepository;

        public Basket? GetUserBasket
        {
            get
            {
                Basket? basket = ContextUser().Result;
                return basket;
            }
        }

        public BasketService(IHttpContextAccessor contextAccessor, UserManager<AppUser> userManager, IOrderReadRepository orderReadRepo, IBasketWriteRepository basketWriteRepository, IBasketItemWriteRepository basketItemWriteRepository, IBasketItemReadRepository basketItemReadRepository, IBasketReadRepository basketReadRepository)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _orderReadRepo = orderReadRepo;
            _basketWriteRepository = basketWriteRepository;
            _basketItemWriteRepository = basketItemWriteRepository;
            _basketItemReadRepository = basketItemReadRepository;
            _basketReadRepository = basketReadRepository;
        }
        private async Task<Basket?> ContextUser()
        {
            var userName = _contextAccessor.HttpContext.User.Identity?.Name ??  null;
            if (!string.IsNullOrEmpty(userName))
            {
                var user = await _userManager.Users.Include(u =>u.Baskets).FirstOrDefaultAsync(u => u.UserName == userName);

                var _basket = from basket in user.Baskets
                              join order in _orderReadRepo.Table
                              on basket.Id equals order.Id into BasketOrders
                              from order in BasketOrders.DefaultIfEmpty()
                              select new
                              {
                                  Basket = basket,
                                  Order = order
                              };

                Basket? targetBasket = null;
                if (_basket.Any(b => b.Order is null))
                {
                    targetBasket = _basket.FirstOrDefault(b => b.Order is null)?.Basket;
                }
                else
                {
                    targetBasket = new();
                    user.Baskets.Add(targetBasket);
                }
                await _basketWriteRepository.SaveAsync();
                return targetBasket;
            }
            throw new Exception("beklenmeyen bir hata ile karşılaşıldı");
        }

        public async Task AddItemToBasketAsync(VM_Create_BasketItem basketItem)
        {
            Basket? basket = await ContextUser();
            if (basket != null)
            {
                BasketItem _basketItem = await _basketItemReadRepository.GetSingleAsync(bi => bi.BasketId == basket.Id && bi.ProductId == Guid.Parse(basketItem.ProductId));
                if (_basketItem != null) {
                    _basketItem.Quantity++;
                }
                else
                {
                    await _basketItemWriteRepository.AddAsync(new()
                    {
                        BasketId = basket.Id,
                        ProductId = Guid.Parse(basketItem.ProductId),
                        Quantity = basketItem.Quantity,
                    });
                }
                await _basketItemWriteRepository.SaveAsync();
            }
        }

        public async Task<List<BasketItem>> GetBasketItemsAsync()
        {
            Basket? basket = await ContextUser();
            var result = await _basketReadRepository.Table.Include(b => b.BasketItems).ThenInclude(bi => bi.Product).FirstOrDefaultAsync(b => b.Id == basket.Id);
            return result.BasketItems.ToList();
        }

        public async Task RemoveBasketItemAsync(string basketItemId)
        {
            BasketItem? basketItem = await _basketItemReadRepository.GetByIdAsync(basketItemId);
            if (basketItem != null)
            {
                _basketItemWriteRepository.Remove(basketItem);
                await _basketItemWriteRepository.SaveAsync();
            }
        }

        public async Task UpdateQuantityAsync(VM_Update_BasketItem basketItem)
        {
            BasketItem? _basketItem = await _basketItemReadRepository.GetByIdAsync(basketItem.BasketItemId);
            if (_basketItem != null)
            {
                _basketItem.Quantity = basketItem.Quantity;
                await _basketItemWriteRepository.SaveAsync();
            }
        }

        
    }
}
