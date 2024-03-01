using ETicaret.Application.Features.Commands.Order.CreateOrder;
using ETicaret.Application.Features.Queries.Order.GetAllOrders;
using ETicaret.Application.Features.Queries.Order.GetOrderById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes ="Admin")]
    public class OrdersController : ControllerBase
    {
        readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody]CreateOrderCommandRequest req)
        {
            var response = await _mediator.Send(req);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders([FromQuery] GetAllOrdersQueryRequest req)
        {
            GetAllOrdersQueryResponse response = await _mediator.Send(req);
            return Ok(response);                  
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById([FromRoute] GetOrderByIdQueryRequest req)
        {
            var response = await _mediator.Send(req);
            return Ok(response);
        }

    }
}
