using ETicaret.Application.Const;
using ETicaret.Application.CustomAttribute;
using ETicaret.Application.Enums;
using ETicaret.Application.Features.Commands.Order.CompleteOrder;
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
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Writing, Definition = "Create Order")]
        public async Task<IActionResult> CreateOrder([FromBody]CreateOrderCommandRequest req)
        {
            var response = await _mediator.Send(req);
            return Ok(response);
        }


        [HttpGet]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Reading, Definition = "Get All Orders")]
        public async Task<IActionResult> GetAllOrders([FromQuery] GetAllOrdersQueryRequest req)
        {
            GetAllOrdersQueryResponse response = await _mediator.Send(req);
            return Ok(response);                  
        }


        [HttpGet("{id}")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Reading, Definition = "Get Order By Id")]
        public async Task<IActionResult> GetOrderById([FromRoute] GetOrderByIdQueryRequest req)
        {
            var response = await _mediator.Send(req);
            return Ok(response);
        }


        [HttpGet("complete-order/{id}")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Updating, Definition = "Complete Order")]
        public async Task<IActionResult> CompleteOrder([FromRoute]CompleteOrderCommandRequest req)
        {
            var response = await _mediator.Send(req);
            return Ok(response);
        }
    }
}
