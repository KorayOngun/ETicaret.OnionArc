using ETicaret.Application.Features.Commands.Basket;
using ETicaret.Application.Features.Commands.Basket.UpdateQuantity;
using ETicaret.Application.Features.Queries.Basket.GetBasketItems;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class BasketsController : ControllerBase
    {
        readonly IMediator _mediator;

        public BasketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasketItems([FromQuery]GetBasketItemsQueryRequest req)
        {
            var response = await _mediator.Send(req);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddItemToBasket(AddItemToBasketCommandRequest req)
        {
            var response = await _mediator.Send(req);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateQuantity(UpdateQuantityCommandRequest req)
        {
            var response = await _mediator.Send(req);
            return Ok(response);
        }

        [HttpDelete("{BasketItemId}")]
        public async Task<IActionResult> RemoveBasketItem([FromRoute] RemoveBasketItemCommandRequest req)
        {
            var response = await _mediator.Send(req);
            return Ok(response);
        }
    }
}
