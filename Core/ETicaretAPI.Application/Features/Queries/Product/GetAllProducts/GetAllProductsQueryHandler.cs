using ETicaretAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ETicaretAPI.Application.Features.Queries.Product.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQueryRequest, GetAllProductsQueryResponse>
    {
        readonly IProductReadRepository productReadRepository;
        readonly ILogger<GetAllProductsQueryHandler> logger;
        public GetAllProductsQueryHandler(IProductReadRepository productReadRepository, ILogger<GetAllProductsQueryHandler> logger)
        {
            this.productReadRepository = productReadRepository;
            this.logger = logger;
        }

        public async Task<GetAllProductsQueryResponse> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Get all products");
            var totalCount = productReadRepository.GetAll(false).Count();
            var products = productReadRepository.GetAll(false).Skip(request.Page * request.Size).Take(request.Size).Include(x=>x.ProductImageFiles).Select(x => new
            {
                x.ID,
                x.Name,
                x.Stock,
                x.Price,
                x.CreatedDate,
                x.UpdatedDate,
                x.ProductImageFiles
            }).ToList();
            return new GetAllProductsQueryResponse
            {
                Products = products,
                TotalCount = totalCount
            };
        }
    }
}
