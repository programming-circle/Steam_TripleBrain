using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Services;
using Steam_TripleBrain.CQRS.Command.Auth;
using LoginRequest = Steam_TripleBrain.Profiles.Tokens.LoginRequest;

namespace Steam_TripleBrain.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator, ITokenService tokenService, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest register)
        {
            if (register == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new RegisterCommand
            {
                Id = Guid.NewGuid(),
                Username = register.Username,
                Email = register.Email,
                Password = register.Password
            };

            var result = await _mediator.Send(command);
            if (result == null)
            {
                return BadRequest("Registration failed");
            }

            return Ok(new
            {
                AccessToken = result.Accesstoken,
                AccessTokenExpireAtUtc = result.AccessExpiresAtUtc,
                RefreshToken = result.RefreshToken,
                RefreshExpireAtUtc = result.RefreshExpiresAtUtc
            });
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            if (login == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new LoginCommand
            {
                Email = login.Email,
                Password = login.Password
            };

            var result = await _mediator.Send(command);
            if (result == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(new
            {
                AccessToken = result.Accesstoken,
                AccessTokenExpireAtUtc = result.AccessExpiresAtUtc,
                RefreshToken = result.RefreshToken,
                RefreshExpireAtUtc = result.RefreshExpiresAtUtc
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            if (refreshRequest == null || string.IsNullOrEmpty(refreshRequest.RefreshToken))
            {
                return BadRequest("Invalid refresh token.");
            }
            var response = await _tokenService.RefreshAsync(refreshRequest.RefreshToken);
            if (response == null)
            {
                return Unauthorized("Invalid or expired refresh token.");
            }
            return Ok(response);
        }

        [Authorize]//logout
        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke(RefreshRequest revokeRequest)
        {
            var succes = await _tokenService.RevokeAsync(revokeRequest.RefreshToken);
            if (!succes)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
