using MediatR;

namespace ETicaretAPI.Application.Features.Queries.ProductImage.GetProductImages
{
    public class GetProductImageQueryRequest : IRequest<List<GetProductImageQueryResponse>>
    {
        public string Id { get; set; }
    }
}
