using Microsoft.AspNetCore.Mvc;
using MediatR;
using Steam_TripleBrain.Services;
using Steam_TripleBrain.CQRS.Command.Auth;
using Steam_TripleBrain.Profiles.Tokens;

namespace Steam_TripleBrain.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest register)
        {
            if (register == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new RegisterCommand
            {
                Id = Guid.NewGuid(),
                Username = register.Username,
                Email = register.Email,
                Password = register.Password
            };

            var result = await _mediator.Send(command);
            if (result == null)
                return BadRequest("Registration failed");

            return Ok(result);
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            if (login == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new LoginCommand
            {
                Email = login.Email,
                Password = login.Password,
                StaySignedIn = login.StaySignedIn
            };

            var result = await _mediator.Send(command);
            if (result == null)
                return Unauthorized("Invalid email or password.");

            return Ok(result);
        }
    }
}
