using BCrypt.Net;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Repositories.Interface;

namespace Steam_TripleBrain.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repo;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthService> _logger;
        private readonly Microsoft.AspNetCore.Identity.UserManager<AppUser> _userManager;

        public AuthService(IUserRepository repo, ITokenService tokenService, ILogger<AuthService> logger,
            Microsoft.AspNetCore.Identity.UserManager<AppUser> userManager)
        {
            _repo = repo;
            _tokenService = tokenService;
            _logger = logger;
            _userManager = userManager;
            _logger.LogInformation("### AuthService start working");
        }

        // Create Identity AppUser from email/password (implements IAuthService)
        public async Task<bool> RegisterAsync(string email, string password)
        {
            _logger.LogInformation("### RegisterAsync start working");
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                _logger.LogInformation("### RegisterAsync: missing email or password");
                return false;
            }

            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                _logger.LogInformation("### RegisterAsync: Identity create failed: {errors}", string.Join(',', result.Errors.Select(e => e.Description)));
                return false;
            }

            // assign default role
            await _userManager.AddToRoleAsync(user, "User");

            _logger.LogInformation("### RegisterAsync: user created successfully.");

            // optionally create tokens and persist refresh token if token service does that
            var access = await _tokenService.CreateAccessTokenAsync(user);
            var refresh = await _tokenService.CreateRefreshTokenAsync(user);

            return true;
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            _logger.LogInformation("### LoginAsync start working");

            var user = await _userManager.FindByNameAsync(username) ?? await _userManager.FindByEmailAsync(username);
            if (user == null)
            {
                _logger.LogInformation("### LoginAsync: user not found");
                return null;
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!passwordValid)
            {
                _logger.LogInformation("### LoginAsync: invalid password");
                return null;
            }

            var access = await _tokenService.CreateAccessTokenAsync(user);
            return access?.Token;
        }
    }

}
