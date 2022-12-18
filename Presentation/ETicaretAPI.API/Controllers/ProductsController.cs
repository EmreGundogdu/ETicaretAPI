using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductWriteRepository productWriteRepository;
        private readonly IProductReadRepository productReadRepository;
        readonly IFileWriteRepository fileWriteRepository;
        readonly IFileReadRepository fileReadRepository;
        readonly IProductImageFileWriteRepository productImageFileWriteRepository;
        readonly IProductImageFileReadRepository productImageFileReadRepository;
        readonly IInvoiceFileReadRepository invoiceFileReadRepository;
        readonly IInvoiceFileWriteRepository invoiceFileWriteRepository;
        readonly IStorageService _storageService;
        readonly IConfiguration configuration;
        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IFileWriteRepository fileWriteRepository, IFileReadRepository fileReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IProductImageFileReadRepository productImageFileReadRepository, IInvoiceFileReadRepository ınvoiceFileReadRepository, IInvoiceFileReadRepository invoiceFileReadRepository, IInvoiceFileWriteRepository invoiceFileWriteRepository, IStorageService storageService, IConfiguration configuration)
        {
            this.productWriteRepository = productWriteRepository;
            this.productReadRepository = productReadRepository;
            this.fileWriteRepository = fileWriteRepository;
            this.fileReadRepository = fileReadRepository;
            this.productImageFileWriteRepository = productImageFileWriteRepository;
            this.productImageFileReadRepository = productImageFileReadRepository;
            this.invoiceFileReadRepository = invoiceFileReadRepository;
            this.invoiceFileWriteRepository = invoiceFileWriteRepository;
            _storageService = storageService;
            this.configuration = configuration;
        }
        [HttpGet]
        public IActionResult Get([FromQuery] Pagination pagination)
        {
            var totalCount = productReadRepository.GetAll(false).Count();
            var products = productReadRepository.GetAll(false).Skip(pagination.Page * pagination.Size).Take(pagination.Size).Select(x => new
            {
                x.ID,
                x.Name,
                x.Stock,
                x.Price,
                x.CreatedDate,
                x.UpdatedDate
            });
            return Ok(new
            {
                totalCount,
                products
            });

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await productReadRepository.GetByIdAsync(id, false));
        }
        [HttpPost]
        public async Task<IActionResult> Post(VM_Create_Product model)
        {
            await productWriteRepository.AddAsync(new()
            {
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock
            });
            await productWriteRepository.SaveAsync();
            return StatusCode((int)HttpStatusCode.Created);
        }
        [HttpPut]
        public async Task<IActionResult> Put(VM_Update_Product model)
        {
            var product = await productReadRepository.GetByIdAsync(model.Id);
            product.Name = model.Name;
            product.Stock = model.Stock;
            product.Price = model.Price;
            await productWriteRepository.SaveAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await productWriteRepository.RemoveAsync(id);
            await productWriteRepository.SaveAsync();
            return NoContent();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Upload(string id)
        {
            List<(string fileName, string pathOrContianerName)> result = await _storageService.UploadAsync("photo-images", Request.Form.Files);
            var product = await productReadRepository.GetByIdAsync(id);
            await productImageFileWriteRepository.AddRangeAsync(result.Select(x => new ProductImageFile
            {
                FileName = x.fileName,
                Path = x.pathOrContianerName,
                Storqage = _storageService.StorageName,
                Products = new List<Product>() { product }
            }).ToList());
            await productImageFileWriteRepository.SaveAsync();
            return Ok();
        }
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetProductImages(string id)
        {
            Product? product = await productReadRepository.Table.Include(x => x.ProductImageFiles).FirstOrDefaultAsync(x => x.ID == Guid.Parse(id));
            return Ok(product.ProductImageFiles.Select(x => new
            {
                Path = $"{configuration["BaseStorageUrl"]}/{x.Path}",
                x.FileName,
                x.ID
            }));
        }
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteProductImage(string id, string imageId)
        {
            Product? product = await productReadRepository.Table.Include(x => x.ProductImageFiles).FirstOrDefaultAsync(x => x.ID == Guid.Parse(id));
            var proudtcImageFile = product.ProductImageFiles.FirstOrDefault(x => x.ID == Guid.Parse(imageId));
            product.ProductImageFiles.Remove(proudtcImageFile);
            await productWriteRepository.SaveAsync();
            return Ok();
        }
    }
}
