using ETicaretAPI.Application.Features.Queries.ProductImage.GetProductImages;
using ETicaretAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using P = ETicaretAPI.Domain.Entities;
namespace ETicaretAPI.Application.Features.Queries.ProductImageFile.GetProductImages
{
    public class GetProductImageQueryHandler : IRequestHandler<GetProductImageQueryRequest, List<GetProductImageQueryResponse>>
    {
        readonly IProductReadRepository productReadRepository;
        readonly IConfiguration configuration;
        public GetProductImageQueryHandler(IProductReadRepository productReadRepository, IConfiguration configuration)
        {
            this.productReadRepository = productReadRepository;
            this.configuration = configuration;
        }

        public async Task<List<GetProductImageQueryResponse>> Handle(GetProductImageQueryRequest request, CancellationToken cancellationToken)
        {

            P::Product? product = await productReadRepository.Table.Include(x => x.ProductImageFiles).FirstOrDefaultAsync(x => x.ID == Guid.Parse(request.Id));
            return product.ProductImageFiles.Select(x => new GetProductImageQueryResponse
            {
                Path = $"{configuration["BaseStorageUrl"]}/{x.Path}",
                FileName= x.FileName,
                Id= x.ID
            }).ToList();
        }
    }
}
