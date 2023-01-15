using ETicaretAPI.Application.Abstractions.Services.Configurations;
using ETicaretAPI.Application.CustomAttributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class ApplicationServicesController : ControllerBase
    {
        readonly IApplicationService applicationService;

        public ApplicationServicesController(IApplicationService applicationService)
        {
            this.applicationService = applicationService;
        }
        [HttpGet]
        [AuthorizeDefinition(ActionType =Application.Enums.ActionType.Reading,Definition ="Get Authorize Definition Endpoints",Menu ="Application Services")]
        public IActionResult GetAuthroizeDefinitionEndpoints(Type type)
        {
            var datas = applicationService.GetAuthorizeDefinitionEndpoints(typeof(Program));
            return Ok(datas);
        }
    }
}
