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
        private readonly IOrderWriteRepository orderWriteRepository;
        private readonly ICustomerWriteRepository customerWriteRepository;
        private readonly IOrderReadRepository orderReadRepository;
        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IOrderWriteRepository orderWriteRepository, ICustomerWriteRepository customerWriteRepository, IOrderReadRepository orderReadRepository)
        {
            this.productWriteRepository = productWriteRepository;
            this.productReadRepository = productReadRepository;
            this.orderWriteRepository = orderWriteRepository;
            this.customerWriteRepository = customerWriteRepository;
            this.orderReadRepository = orderReadRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("M45hqbqW");
        }
    }
}
