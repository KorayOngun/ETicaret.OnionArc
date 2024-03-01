using ETicaret.Application.Repositories;
using ETicaret.Application.RequestParameters;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Application.Features.Queries.Product.GetAllProduct
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
    {

        readonly IProductReadRepository _productReadRepository;
        private readonly ILogger<GetAllProductQueryHandler> _logger;
        public GetAllProductQueryHandler(IProductReadRepository productReadRepository, ILogger<GetAllProductQueryHandler> logger)
        {
            _productReadRepository = productReadRepository;
            _logger = logger;
        }

        public async Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
        {
            var totalProductCount = _productReadRepository.GetAll(false).Count();
            
            var products = await _productReadRepository.GetAll(false).Skip(request.Page * request.Size).Take(request.Size)
                .Include(p => p.ProductImages)
                .Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDate,
                p.UpdatedDate,
                p.ProductImages
            }).ToListAsync();

            _logger.LogInformation("ürünleri listeledi");
            return new()
            {
                Products = products,
                TotalProductCount = totalProductCount
            };
        }
    }
}
