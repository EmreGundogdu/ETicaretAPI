using ETicaretAPI.Application.Repositories;
using MediatR;

namespace ETicaretAPI.Application.Features.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQueryRequest, GetAllProductsQueryResponse>
    {
        readonly IProductReadRepository productReadRepository;

        public GetAllProductsQueryHandler(IProductReadRepository productReadRepository)
        {
            this.productReadRepository = productReadRepository;
        }

        public async Task<GetAllProductsQueryResponse> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
        {

            var totalCount = productReadRepository.GetAll(false).Count();
            var products = productReadRepository.GetAll(false).Skip(request.Page * request.Size).Take(request.Size).Select(x => new
            {
                x.ID,
                x.Name,
                x.Stock,
                x.Price,
                x.CreatedDate,
                x.UpdatedDate
            }).ToList();
            return new GetAllProductsQueryResponse
            {
                Products = products,
                TotalCount = totalCount
            };
        }
    }
}
