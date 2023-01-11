using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Features.Queries.Product.GetAllProducts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.Order.GetOrders
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQueryRequest, GetAllOrdersQueryResponse>
    {
        readonly IOrderService orderService;

        public GetAllOrdersQueryHandler(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        public async Task<GetAllOrdersQueryResponse> Handle(GetAllOrdersQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await orderService.GetAllOrdersAsync(request.Page, request.PageSize);
            return new()
            {
                TotalCount = data.TotalCount,
                Orders = data.Orders
            };
        }
    }
}
