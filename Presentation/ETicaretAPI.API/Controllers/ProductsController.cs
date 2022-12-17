using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Application.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductWriteRepository productWriteRepository;
        private readonly IProductReadRepository productReadRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository,IWebHostEnvironment webHostEnvironment)
        {
            this.productWriteRepository = productWriteRepository;
            this.productReadRepository = productReadRepository;
            this.webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Get([FromQuery]Pagination pagination)
        {
            var totalCount = productReadRepository.GetAll(false).Count();
           var products =  productReadRepository.GetAll(false).Skip(pagination.Page * pagination.Size).Take(pagination.Size).Select(x => new
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
        public async Task<IActionResult> Upload()
        {
            string uploadPath = Path.Combine(webHostEnvironment.WebRootPath, "resource/product-images");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            Random random = new();
            foreach (IFormFile file in Request.Form.Files)
            {
                string fullPath = Path.Combine(uploadPath, $"{random.Next()}{Path.GetExtension(file.FileName)}");
                using FileStream fileStream = new(fullPath, FileMode.Create, FileAccess.Write,FileShare.None, 1024 * 1024, useAsync: false);
                file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }
            return Ok();
        }    
    }
}
