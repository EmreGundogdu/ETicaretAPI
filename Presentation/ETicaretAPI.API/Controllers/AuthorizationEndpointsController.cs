using ETicaretAPI.Application.Features.Commands.AuthorizationEndpoints.AssignRoleEndpoint;
using ETicaretAPI.Application.Features.Queries.GetRolesToEndpoints;
using ETicaretAPI.Application.Features.Queries.Role.GetRoles;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationEndpointsController : ControllerBase
    {
        readonly IMediator mediator;

        public AuthorizationEndpointsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(AssignRoleEndpointCommandRequest assignRoleEndpointCommandRequest)
        {
            assignRoleEndpointCommandRequest.Type = typeof(Program);
            var response = await mediator.Send(assignRoleEndpointCommandRequest);
            return Ok(response);
        }

        [HttpPost("get-roles-to-endpoint")]
        public async Task<IActionResult> GetRolesToEndpoints(GetRolesToEndpointQueryRequest getRolesToEndpointQueryRequest)
        {
            var response = await mediator.Send(getRolesToEndpointQueryRequest);
            return Ok(response);
        }
    }
}
