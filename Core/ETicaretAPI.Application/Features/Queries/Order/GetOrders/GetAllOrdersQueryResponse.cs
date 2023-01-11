namespace ETicaretAPI.Application.Features.Queries.Order.GetOrders
{
    public class GetAllOrdersQueryResponse
    {
        public int TotalCount { get; set; }
        public object Orders { get; set; }
    }
}