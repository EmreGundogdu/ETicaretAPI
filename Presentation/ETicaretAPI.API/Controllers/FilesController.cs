using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        readonly IConfiguration configuration;

        public FilesController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetBaseUrl()
        {
            return Ok(configuration["BaseStorageUrl"]);
        }
    }
}
