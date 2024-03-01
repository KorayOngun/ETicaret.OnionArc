using ETicaret.Application.Abstractions.Services;
using ETicaret.Application.DTOs.Order;
using ETicaret.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Persistence.Services
{
    public class OrderService : IOrderService
    {
        readonly IOrderWriteRepository _writeRepo;
        readonly IOrderReadRepository _readRepo;

        public OrderService(IOrderWriteRepository writeRepo, IOrderReadRepository readRepo)
        {
            _writeRepo = writeRepo;
            _readRepo = readRepo;
        }

        public async Task CreateOrderAsync(CreateOrder createOrder)
        {
            // 0.34 * 100 => 34.0
            var orderCode = (new Random().NextDouble() * 10000).ToString();
            orderCode = orderCode.Substring(5,orderCode.Length - 5);
            await _writeRepo.AddAsync(new()
            {
                Address = createOrder.Address,
                Id = Guid.Parse(createOrder.BasketId),
                Description = createOrder.Description,
                OrderCode = orderCode
            }); ; ;
            await _writeRepo.SaveAsync();
        }

        public async Task<ListOrder> GetAllOrdersAsync(int page, int size)
        {
            var query =  _readRepo.Table.Include(o => o.Basket).ThenInclude(b => b.User)
                    .Include(o => o.Basket).ThenInclude(b => b.BasketItems).ThenInclude(bi => bi.Product);
            var count = await query.CountAsync();
            var data = await query.Skip(page * size).Take(size).Select(o => new
                                    {
                                        o.Id,
                                        o.Basket.User.UserName,
                                        o.CreatedDate,
                                        o.OrderCode,
                                        TotalPrice = o.Basket.BasketItems.Sum(bi => bi.Product.Price * bi.Quantity)
                                    }).ToListAsync();
            return new()
            {
                Orders = data,
                TotalOrderCount = count
            };
        }

        public async Task<SingleOrder> GetOrderByIdAsync(string id)
        {
            var data = await _readRepo.Table
                                .Include(o => o.Basket)
                                .ThenInclude(b => b.BasketItems)
                                .ThenInclude(bi => bi.Product)
                                .FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));

            return new()
            {
                Id = data.Id.ToString(),
                BasketItems = data.Basket.BasketItems.Select(bi => new
                {
                    bi.Product.Name,
                    bi.Product.Price,
                    bi.Quantity,
                }),
                Address = data.Address,
                CreatedDate = data.CreatedDate,
                OrderCode = data.OrderCode,
                Description = data.Description
            };
                
        }
    }
}




