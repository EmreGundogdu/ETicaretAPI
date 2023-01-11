using ETicaretAPI.Application.Features.Commands.Order.CreateOrder;
using ETicaretAPI.Application.Features.Queries.Order.GetOrderById;
using ETicaretAPI.Application.Features.Queries.Order.GetOrders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class OrdersController : ControllerBase
    {
        readonly IMediator mediator;

        public OrdersController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] GetAllOrdersQueryRequest getAllOrdersQueryRequest)
        {
            var result = await mediator.Send(getAllOrdersQueryRequest);
            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetOrderById([FromRoute] GetOrderByIdQueryRequest getOrderByIdQueryRequest)
        {
            var result = await mediator.Send(getOrderByIdQueryRequest);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderCommandRequest createOrderCommandRequest)
        {
            var result = await mediator.Send(createOrderCommandRequest);
            return Ok(result);
        }
    }
}
