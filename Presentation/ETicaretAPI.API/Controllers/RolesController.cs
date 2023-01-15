using ETicaretAPI.Application.CustomAttributes;
using ETicaretAPI.Application.Features.Commands.Role.CreateRole;
using ETicaretAPI.Application.Features.Commands.Role.DeleteRole;
using ETicaretAPI.Application.Features.Commands.Role.UpdateRole;
using ETicaretAPI.Application.Features.Queries.Role.GetRole;
using ETicaretAPI.Application.Features.Queries.Role.GetRoles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class RolesController : ControllerBase
    {
        readonly IMediator mediator;

        public RolesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [AuthorizeDefinition(ActionType =Application.Enums.ActionType.Reading,Definition ="Get Roles",Menu ="Roles")]
        public async Task<IActionResult> GetRoles([FromQuery] GetRolesQueryRequest getRolesQueryRequest)
        {
            var response = await mediator.Send(getRolesQueryRequest);
            return Ok(response);

        }
        [HttpGet("{Id}")]
        [AuthorizeDefinition(ActionType = Application.Enums.ActionType.Reading, Definition = "Get Role", Menu = "Roles")]
        public async Task<IActionResult> GetRole([FromRoute] GetRoleQueryRequest getRoleQueryRequest)
        {
            var response = await mediator.Send(getRoleQueryRequest);
            return Ok(response);

        }
        [HttpPost]
        [AuthorizeDefinition(ActionType = Application.Enums.ActionType.Writing, Definition = "Create Role", Menu = "Roles")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommandRequest createRoleCommandRequest)
        {
            var response = await mediator.Send(createRoleCommandRequest);
            return Ok(response);

        }

        [HttpPut("{Id}")]
        [AuthorizeDefinition(ActionType = Application.Enums.ActionType.Updating, Definition = "Update Role", Menu = "Roles")]
        public async Task<IActionResult> UpdateRole([FromBody, FromRoute] UpdateRoleCommandRequest updateRoleCommandRequest)
        {
            var response = await mediator.Send(updateRoleCommandRequest);
            return Ok(response);

        }

        [HttpPut("{name}")]
        [AuthorizeDefinition(ActionType = Application.Enums.ActionType.Deleting, Definition = "Delete Role", Menu = "Roles")]
        public async Task<IActionResult> DeleteRole([FromRoute] DeleteRoleCommandRequest deleteRoleCommandRequest)
        {
            var response = await mediator.Send(deleteRoleCommandRequest);
            return Ok(response);
        }
    }
}
