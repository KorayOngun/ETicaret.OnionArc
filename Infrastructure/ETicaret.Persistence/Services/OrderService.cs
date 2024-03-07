using ETicaret.Application.Abstractions.Services;
using ETicaret.Application.DTOs.Order;
using ETicaret.Application.Repositories;
using ETicaret.Domain.Entities;
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
        readonly ICompletedOrderReadRepository _completedOrderReadRepository;
        readonly ICompletedOrderWriteRepository _completedOrderWriteRepository;

        public OrderService(IOrderWriteRepository writeRepo, IOrderReadRepository readRepo, ICompletedOrderReadRepository completedOrderReadRepository, ICompletedOrderWriteRepository completedOrderWriteRepository)
        {
            _writeRepo = writeRepo;
            _readRepo = readRepo;
            _completedOrderReadRepository = completedOrderReadRepository;
            _completedOrderWriteRepository = completedOrderWriteRepository;
        }

        public async Task<(bool, CompletedOrderDto)> CompleteOrderAsync(string id)
        {
            Order? order = await _readRepo.Table.Include(o => o.Basket).ThenInclude(b => b.User).FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));

            if (order != null)
            {
                await _completedOrderWriteRepository.AddAsync(new()
                {
                    OrderId = Guid.Parse(id)
                });
                return ((await _completedOrderWriteRepository.SaveAsync()) > 0, new()
                {
                    OrderCode = order.OrderCode,
                    OrderDate = order.CreatedDate,
                    UserNameSurname = order.Basket.User.NameSurname,
                    Email = order.Basket.User.Email
                });
            }
            return (false,default);
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
            var data =  query.Skip(page * size).Take(size);


            var data2 = from order in query
                        join completedOrder in _completedOrderReadRepository.Table
                        on order.Id equals completedOrder.OrderId into co
                        from _co in co.DefaultIfEmpty()
                        select new
                        {
                            order.Id,
                            order.CreatedDate,
                            order.OrderCode,
                            order.Basket,
                            Completed = _co != null ? true : false,
                        };

            return new()
            {
                Orders = await data2.Select(o => new
                {
                    o.Id,
                    o.Basket.User.UserName,
                    o.CreatedDate,
                    o.OrderCode,
                    TotalPrice = o.Basket.BasketItems.Sum(bi => bi.Product.Price * bi.Quantity),
                    o.Completed
                }).ToListAsync(),
                TotalOrderCount = count
            };
        }

        public async Task<SingleOrder> GetOrderByIdAsync(string id)
        {
            var data = _readRepo.Table
                                .Include(o => o.Basket)
                                .ThenInclude(b => b.BasketItems)
                                .ThenInclude(bi => bi.Product);

            var data2 = await (from order in data
                        join completedOrder in _completedOrderReadRepository.Table
                        on order.Id equals completedOrder.OrderId into co
                        from _co in co.DefaultIfEmpty()
                        select new
                        {
                            order.Id,
                            order.CreatedDate,
                            order.OrderCode,
                            order.Basket,
                            Completed = _co != null ? true : false,
                            order.Address,
                            order.Description
                        }).FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));
                            
                                
                                
                                //.FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));



            return new()
            {
                Id = data2.Id.ToString(),
                BasketItems = data2.Basket.BasketItems.Select(bi => new
                {
                    bi.Product.Name,
                    bi.Product.Price,
                    bi.Quantity,
                }),
                Address = data2.Address,
                CreatedDate = data2.CreatedDate,
                OrderCode = data2.OrderCode,
                Description = data2.Description,
                Completed = data2.Completed
            };
                
        }
    }
}




