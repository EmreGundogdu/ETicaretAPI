using ETicaretAPI.Application.CustomAttributes;
using ETicaretAPI.Application.Features.Commands.AppUser.CreateUser;
using ETicaretAPI.Application.Features.Commands.AppUser.UpdatePassword;
using ETicaretAPI.Application.Features.Commands.User.AssignRoleToUser;
using ETicaretAPI.Application.Features.Queries.User.GetAllUsers;
using ETicaretAPI.Application.Features.Queries.User.GetRolesToUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly IMediator mediator;

        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommandRequest createUserCommandRequest)
        {
            var respose = await mediator.Send(createUserCommandRequest);
            return Ok(respose);
        }

        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordCommandRequest updatePasswordCommandRequest)
        {
            var response = await mediator.Send(updatePasswordCommandRequest);
            return Ok(response);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = Application.Enums.ActionType.Reading, Definition = "Get All Users", Menu = "Users")]
        public async Task<IActionResult> GetAllUsers([FromQuery]GetAllUsersQueryRequest getAllUsersQueryRequest)
        {
            var response = await mediator.Send(getAllUsersQueryRequest);
            return Ok(response);
        }

        [HttpPost("assign-role-to-user")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = Application.Enums.ActionType.Writing, Definition = "Assign Role To Users", Menu = "Users")]
        public async Task<IActionResult> AssignRoleToUser(AssignRoleToUserCommandRequest assignRoleToUserCommandRequest)
        {
            var responee = await mediator.Send(assignRoleToUserCommandRequest);
            return Ok(responee);
        }

        [HttpGet("get-roles-to-user/{UserId}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = Application.Enums.ActionType.Reading, Definition = "Get Roles To User", Menu = "Users")]
        public async Task<IActionResult> GetRolesToUser([FromRoute]GetRolesToUserQueryRequest getRolesToUserQueryRequest)
        {
            var response = await mediator.Send(getRolesToUserQueryRequest);
            return Ok(response);
        }
    }
}
