using ETicaretAPI.Application.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductWriteRepository productWriteRepository;
        private readonly IProductReadRepository productReadRepository;
        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository)
        {
            this.productWriteRepository = productWriteRepository;
            this.productReadRepository = productReadRepository;
        }
        [HttpGet]
        public async Task Get()
        {
            //await productWriteRepository.AddRangeAsync(new()
            //{
            //    new(){ID=Guid.NewGuid(),Name="Product 1",Price=100,CreatedDate=DateTime.UtcNow,Stock=10},
            //    new(){ID=Guid.NewGuid(),Name="Product 2",Price=200,CreatedDate=DateTime.UtcNow,Stock=20},
            //    new(){ID=Guid.NewGuid(),Name="Product 3",Price=300,CreatedDate=DateTime.UtcNow,Stock=30}
            //});
            //await productWriteRepository.SaveAsync();
            var p = await productReadRepository.GetByIdAsync("4f88d34a-9605-4144-b3c1-35851778d4bf");
            p.Name = "Product 1";
            await productWriteRepository.SaveAsync();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await productReadRepository.GetByIdAsync(id);
            return Ok(result);
        }
    }
}
