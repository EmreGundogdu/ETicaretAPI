using ETicaretAPI.Application.Abstractions.Services;
using MediatR;

namespace ETicaretAPI.Application.Features.Queries.User.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQueryRequest, GetAllUsersQueryResponse>
    {
        readonly IUserService userService;

        public GetAllUsersQueryHandler(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<GetAllUsersQueryResponse> Handle(GetAllUsersQueryRequest request, CancellationToken cancellationToken)
        {
            var users = await userService.GetAllUsersAsync(request.Page, request.PageSize);
            return new()
            {
                Users = users,
                TotalCount = userService.TotalUsersCount
            };
        }
    }
}
