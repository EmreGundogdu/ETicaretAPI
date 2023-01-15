using MediatR;

namespace ETicaretAPI.Application.Features.Queries.Role.GetRole
{
    public class GetRoleQueryRequest : IRequest<GetRoleQueryResponse>
    {
        public string Id { get; set; }
    }
}