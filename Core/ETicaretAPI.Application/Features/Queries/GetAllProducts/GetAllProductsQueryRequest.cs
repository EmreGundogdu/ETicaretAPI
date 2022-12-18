using ETicaretAPI.Application.RequestParameters;
using MediatR;

namespace ETicaretAPI.Application.Features.Queries.GetAllProducts
{
    public class GetAllProductsQueryRequest : IRequest<GetAllProductsQueryResponse>
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 5;
    }
}
