using ETicaretAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.ProductImage.ChangeShowcaseImage
{
    public class ChangeShowcaseImageCommandHandler : IRequestHandler<ChangeShowcaseImageCommandRequest, ChangeShowcaseImageCommandResponse>
    {
        readonly IProductImageFileWriteRepository productImageFileWriteRepository;

        public ChangeShowcaseImageCommandHandler(IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            this.productImageFileWriteRepository = productImageFileWriteRepository;
        }

        public async Task<ChangeShowcaseImageCommandResponse> Handle(ChangeShowcaseImageCommandRequest request, CancellationToken cancellationToken)
        {
            var query = productImageFileWriteRepository.Table.Include(x => x.Products).SelectMany(x => x.Products, (pif, p) => new
            {
                pif,
                p
            });
            var data = await query.FirstOrDefaultAsync(x => x.p.ID == Guid.Parse(request.ProductId) && x.pif.Showcase);
            if (data != null)
                data.pif.Showcase = false;
            var image = await query.FirstOrDefaultAsync(x => x.pif.ID == Guid.Parse(request.ImageId));
            if (image != null)
                image.pif.Showcase = true;
            await productImageFileWriteRepository.SaveAsync();
            return new();
        }
    }
}
