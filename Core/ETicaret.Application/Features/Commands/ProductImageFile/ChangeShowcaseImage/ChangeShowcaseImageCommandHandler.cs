using ETicaret.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Application.Features.Commands.ProductImageFile.ChangeShowcaseImage
{
    public class ChangeShowcaseImageCommandHandler : IRequestHandler<ChangeShowcaseImageCommandRequest, ChangeShowcaseImageCommandResponse>
    {
        private readonly IProductImageFileWriteRepository _readRepo;

        public ChangeShowcaseImageCommandHandler(IProductImageFileWriteRepository readRepo)
        {
            _readRepo = readRepo;
        }

        public async Task<ChangeShowcaseImageCommandResponse> Handle(ChangeShowcaseImageCommandRequest request, CancellationToken cancellationToken)
        {
            var query = _readRepo.Table.Include(t => t.Product).SelectMany(p => p.Product, (pif, p) => new
            {
                pif,
                p
            });

            var data = await query.FirstOrDefaultAsync(p => p.p.Id == Guid.Parse(request.ProductId) && p.pif.Showcase == true);

            if (data != null)
                data.pif.Showcase = false;

            var image = await query.FirstOrDefaultAsync(p => p.pif.Id == Guid.Parse(request.ImageId));

            if (image != null)
                image.pif.Showcase = true;

            await _readRepo.SaveAsync();

            return new();

        }
    }
}
