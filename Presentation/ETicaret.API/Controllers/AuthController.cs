using ETicaret.Application.Abstractions.Services;
using ETicaret.Application.Features.Commands.AppUser.GoogleLogin;
using ETicaret.Application.Features.Commands.AppUser.LoginUser;
using ETicaret.Application.Features.Commands.AppUser.RefreshTokenLogin;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginUserCommandRequest req)
        {
            var response = await _mediator.Send(req);
            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshTokenLogin([FromBody] RefreshTokenLoginCommandRequest req)
        {
            var response = await _mediator.Send(req);
            return Ok(response);
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginCommandRequest req)
        {
            var response = await _mediator.Send(req);
            return Ok(response);
        }
    }
}
