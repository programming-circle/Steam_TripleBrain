using MediatR;
using Steam_TripleBrain.CQRS.Command;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Profiles.Tokens;
using Microsoft.AspNetCore.Identity;
using Steam_TripleBrain.Services;

namespace Steam_TripleBrain.CQRS.Handler.Auth
{
    public class LoginProfileHandler : IRequestHandler<LoginProfileCommand, AuthResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<LoginProfileHandler> _logger;

        public LoginProfileHandler(UserManager<AppUser> userManager, ITokenService tokenService, ILogger<LoginProfileHandler> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<AuthResponse> Handle(LoginProfileCommand request, CancellationToken cancellationToken)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Username and password are required");

            var user = await _userManager.FindByNameAsync(request.Username) ?? await _userManager.FindByEmailAsync(request.Username);
            if (user == null)
            {
                _logger.LogInformation("Login: user not found");
                return null;
            }

            var valid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!valid)
            {
                _logger.LogInformation("Login: invalid password");
                return null;
            }

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
