using MediatR;

namespace ETicaretAPI.Application.Features.Commands.AuthorizationEndpoints.AssignRoleEndpoint
{
    public class AssignRoleEndpointCommandRequest:IRequest<AssignRoleEndpointCommandResponse>
    {
        public string[] Roles { get; set; }
        public string EndpointCode { get; set; }
    }
}