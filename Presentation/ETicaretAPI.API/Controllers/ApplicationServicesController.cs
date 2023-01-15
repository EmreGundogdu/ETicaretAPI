using ETicaretAPI.Application.Abstractions.Services.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationServicesController : ControllerBase
    {
        readonly IApplicationService applicationService;

        public ApplicationServicesController(IApplicationService applicationService)
        {
            this.applicationService = applicationService;
        }
        [HttpGet]
        public IActionResult GetAuthroizeDefinitionEndpoints(Type type)
        {
            var datas = applicationService.GetAuthorizeDefinitionEndpoints(typeof(Program));
            return Ok(datas);
        }
    }
}
