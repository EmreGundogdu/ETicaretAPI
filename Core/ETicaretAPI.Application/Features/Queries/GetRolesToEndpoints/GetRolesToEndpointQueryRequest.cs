using MediatR;

namespace ETicaretAPI.Application.Features.Queries.GetRolesToEndpoints
{
    public class GetRolesToEndpointQueryRequest:IRequest<GetRolesToEndpointQueryResponse>
    {
        public string Code { get; set; }
        public string Menu { get; set; }
    }
}