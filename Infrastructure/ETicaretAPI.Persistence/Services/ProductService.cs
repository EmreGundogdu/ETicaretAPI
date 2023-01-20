using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Services
{
    public class ProductService : IProductService
    {
        readonly IProductReadRepository productReadRepository;
        readonly IQRCodeService qRCodeService;
        public ProductService(IProductReadRepository productReadRepository, IQRCodeService qRCodeService)
        {
            this.productReadRepository = productReadRepository;
            this.qRCodeService = qRCodeService;
        }

        public async Task<byte[]> QrCodeToProductAsync(string productId)
        {
            var product = await productReadRepository.GetByIdAsync(productId);
            if (product is null)
            {
                throw new Exception("Not found");
            }
            var plainObject = new
            {
                product.ID,
                product.Name,
                product.Price,
                product.Stock,
                product.CreatedDate
            };
            string platinText = JsonSerializer.Serialize(plainObject);
            return qRCodeService.GenerateQRCode(platinText);
        }
    }
}
