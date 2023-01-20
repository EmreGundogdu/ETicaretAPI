using ETicaretAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.User.GetRolesToUser
{
    public class GetRolesToUserQueryHandler : IRequestHandler<GetRolesToUserQueryRequest, GetRolesToUserQueryResponse>
    {
        readonly IUserService userService;

        public GetRolesToUserQueryHandler(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<GetRolesToUserQueryResponse> Handle(GetRolesToUserQueryRequest request, CancellationToken cancellationToken)
        {
            var response = await userService.GetRolesToUserAsync(request.UserId);
            return new()
            {
                UserRoles = response
            };
        }
    }
}
