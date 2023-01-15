using ETicaretAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.Role.GetRole
{
    public class GetRoleQueryHandler : IRequestHandler<GetRoleQueryRequest, GetRoleQueryResponse>
    {
        readonly IRoleService roleService;

        public GetRoleQueryHandler(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        public async Task<GetRoleQueryResponse> Handle(GetRoleQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await roleService.GetRoleById(request.Id);
            return new()
            {
                Id = result.id,
                Name = result.name
            };
        }
    }
}
