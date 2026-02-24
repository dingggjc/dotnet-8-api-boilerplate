using ErrorOr;
using Flow.Application.Commands.Auth;
using Flow.Application.DTO.Auth;
using MediatR;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;

namespace Flow.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator; 

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            var command = new LoginUserCommand(request);
            var result = await _mediator.Send(command);

            return result.Match(
                response => Ok(response),
                errors => Problem(errors.First().Description));  
        }
    }
}