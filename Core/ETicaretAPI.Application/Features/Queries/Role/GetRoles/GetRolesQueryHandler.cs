using ETicaretAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.Role.GetRoles
{
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQueryRequest, GetRolesQueryResponse>
    {
        readonly IRoleService roleService;

        public GetRolesQueryHandler(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        public async Task<GetRolesQueryResponse> Handle(GetRolesQueryRequest request, CancellationToken cancellationToken)
        {
            var (datas,count) =  roleService.GetAllRoles(request.Page,request.PageSize);
            return new()
            {
                Datas = datas,
                TotalCount = count
            };
        }
    }
}
