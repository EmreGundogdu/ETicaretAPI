using MediatR;

namespace ETicaretAPI.Application.Features.Queries.Role.GetRoles
{
    public class GetRolesQueryRequest : IRequest<GetRolesQueryResponse>
    {
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 5;
    }
}