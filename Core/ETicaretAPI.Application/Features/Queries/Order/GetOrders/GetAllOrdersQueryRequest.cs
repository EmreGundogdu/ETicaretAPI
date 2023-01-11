using MediatR;

namespace ETicaretAPI.Application.Features.Queries.Order.GetOrders
{
    public class GetAllOrdersQueryRequest:IRequest<GetAllOrdersQueryResponse>
    {
        public int Page { get; set; } = 0;

        public int PageSize { get; set; } = 5;
    }
}