using ETicaretAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using P = ETicaretAPI.Domain.Entities;
namespace ETicaretAPI.Application.Features.Commands.ProductImage.RemoveProductImage
{
    public class RemoveProductImageCommandHandler : IRequestHandler<RemoveProductImageCommandRequest, RemoveProductImageCommandResponse>
    {
        readonly IProductReadRepository productReadRepository;
        readonly IProductWriteRepository productWriteRepository;

        public RemoveProductImageCommandHandler(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            this.productReadRepository = productReadRepository;
            this.productWriteRepository = productWriteRepository;
        }

        public async Task<RemoveProductImageCommandResponse> Handle(RemoveProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            P::Product? product = await productReadRepository.Table.Include(x => x.ProductImageFiles).FirstOrDefaultAsync(x => x.ID == Guid.Parse(request.Id));
            var productImageFile = product?.ProductImageFiles.FirstOrDefault(x => x.ID == Guid.Parse(request.ImageId));
            if (productImageFile != null)
                product?.ProductImageFiles.Remove(productImageFile);
            await productWriteRepository.SaveAsync();
            return new();
        }
    }
}
