using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using MediatR;
using P = ETicaretAPI.Domain.Entities;

namespace ETicaretAPI.Application.Features.Commands.ProductImage.UploadProductImage
{
    public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
    {
        readonly IStorageService storageService;
        readonly IProductReadRepository productReadRepository;
        IProductImageFileWriteRepository productImageFileWriteRepository;

        public UploadProductImageCommandHandler(IStorageService storageService, IProductReadRepository productReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            this.storageService = storageService;
            this.productReadRepository = productReadRepository;
            this.productImageFileWriteRepository = productImageFileWriteRepository;
        }

        public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            List<(string fileName, string pathOrContianerName)> result = await storageService.UploadAsync("photo-images", request.Files);
            var product = await productReadRepository.GetByIdAsync(request.Id);
            await productImageFileWriteRepository.AddRangeAsync(result.Select(x => new ProductImageFile
            {
                FileName = x.fileName,
                Path = x.pathOrContianerName,
                Storqage = storageService.StorageName,
                Products = new List<P.Product>() { product }
            }).ToList());
            await productImageFileWriteRepository.SaveAsync();
            return new();
        }
    }
}
