using MediatR;

namespace ETicaretAPI.Application.Features.Queries.User.GetAllUsers
{
    public class GetAllUsersQueryRequest:IRequest<GetAllUsersQueryResponse>
    {
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 0;
    }
}