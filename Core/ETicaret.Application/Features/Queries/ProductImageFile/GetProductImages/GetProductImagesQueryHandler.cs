using ETicaret.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Application.Features.Queries.ProductImageFile.GetProductImages
{
    public class GetProductImagesQueryHandler : IRequestHandler<GetProductImagesQueryRequest, List<GetProductImagesQueryResponse>>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IConfiguration _configuration;

        public GetProductImagesQueryHandler(IProductReadRepository productReadRepository, IConfiguration configuration)
        {
            _productReadRepository = productReadRepository;
            _configuration = configuration;
        }

        public async Task<List<GetProductImagesQueryResponse>> Handle(GetProductImagesQueryRequest request, CancellationToken cancellationToken)
        {
            var  product = await _productReadRepository.Table.Where(p => p.Id == Guid.Parse(request.Id))
                                                       .Include(p => p.ProductImages)
                                                       .FirstOrDefaultAsync();


           return product.ProductImages.Select(p=>new GetProductImagesQueryResponse
           {
               FileName = p.FileName,
               Id = p.Id,
               Path = $"{_configuration["BaseStorageUrl"]}/{p.Path}/{p.FileName}"
           }).ToList();
        }
    }
}
