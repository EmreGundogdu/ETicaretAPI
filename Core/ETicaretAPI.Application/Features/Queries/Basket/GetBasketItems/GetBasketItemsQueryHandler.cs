using ETicaretAPI.Application.Abstractions.Services;
using MediatR;

namespace ETicaretAPI.Application.Features.Queries.Basket.GetBasketItems
{
    public class GetBasketItemsQueryHandler : IRequestHandler<GetBasketItemsQueryRequest, List<GetBasketItemsQueryResponse>>
    {
        readonly IBasketService basketService;

        public GetBasketItemsQueryHandler(IBasketService basketService)
        {
            this.basketService = basketService;
        }

        public async Task<List<GetBasketItemsQueryResponse>> Handle(GetBasketItemsQueryRequest request, CancellationToken cancellationToken)
        {
            var basketItems = await basketService.GetBasketItems();
            return basketItems.Select(x => new GetBasketItemsQueryResponse
            {
                BasketItemId = x.ID.ToString(),
                Name = x.Product.Name,
                Price = x.Product.Price,
                Quantity = x.Quantity
            }).ToList();
        }
    }
}
