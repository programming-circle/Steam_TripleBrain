using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Auth;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Profiles.Tokens;
using Steam_TripleBrain.Services;

namespace Steam_TripleBrain.CQRS.Handler.Auth
{
    public class LoginHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<LoginHandler> _logger;

        public LoginHandler(UserManager<AppUser> userManager, ITokenService tokenService, ILogger<LoginHandler> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("### Handler Login for email: {Email}", request.Email);

            if (request == null)
            {
                _logger.LogWarning("### Login request is null");
                throw new ArgumentException("Login data is required");
            }

            // Find user by email
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("### User not found with email: {Email}", request.Email);
                return null;
            }

            // Check password
            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                _logger.LogWarning("### Invalid password for user: {Email}", request.Email);
                return null;
            }

            _logger.LogInformation("### User {Email} logged in successfully", request.Email);

            // Generate tokens
            var access = await _tokenService.CreateAccessTokenAsync(user);
            var refresh = await _tokenService.CreateRefreshTokenAsync(user);

            return new AuthResponse
            {
                Accesstoken = access?.Token,
                AccessExpiresAtUtc = access?.ExpiresAtUtc ?? DateTime.UtcNow,
                RefreshToken = refresh?.Token,
                RefreshExpiresAtUtc = refresh?.ExpireAtUtc ?? DateTime.UtcNow
            };
        }
    }
}
