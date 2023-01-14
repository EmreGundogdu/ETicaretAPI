using ETicaretAPI.Application.Features.Commands.Order.CompleteOrder;
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
            var response = await mediator.Send(getAllOrdersQueryRequest);
            return Ok(response);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetOrderById([FromRoute] GetOrderByIdQueryRequest getOrderByIdQueryRequest)
        {
            var response = await mediator.Send(getOrderByIdQueryRequest);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderCommandRequest createOrderCommandRequest)
        {
            var response = await mediator.Send(createOrderCommandRequest);
            return Ok(response);
        }

        [HttpGet("complete-order/{Id}")]
        public async Task<IActionResult> CompleteOrder([FromRoute] CompleteOrderCommandRequest completeOrderCommandRequest)
        {
            var response = await mediator.Send(completeOrderCommandRequest);
            return Ok(response);
        }
    }
}
