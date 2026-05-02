using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Auth;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles.Tokens;
using Steam_TripleBrain.Services;

namespace Steam_TripleBrain.CQRS.Handler.Auth
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, JwtTokenProfile?>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<RegisterHandler> _logger;
        private readonly AppDbContext _context;

        public RegisterHandler(
            UserManager<AppUser> userManager,
            ITokenService tokenService,
            ILogger<RegisterHandler> logger,
            AppDbContext context)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _logger = logger;
            _context = context;
        }

        public async Task<JwtTokenProfile?> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("### Handler Registration");

            if (request == null)
                throw new ArgumentException("Registration data is required");

            var exists = await _context.Users.AnyAsync(
                g => g.UserName == request.Username || g.Email == request.Email, cancellationToken);

            if (exists)
            {
                _logger.LogInformation("### User {Username} / {Email} already exists", request.Username, request.Email);
                return null;
            }

            var appUser = UserMappingProfile.ToAppUser(request);

            var result = await _userManager.CreateAsync(appUser, request.Password);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Register failed: {errors}", string.Join(',', result.Errors.Select(e => e.Description)));
                return null;
            }

            var utcNow = DateTime.UtcNow;
            var user = new User
            {
                Id = appUser.Id,
                UserName = appUser.UserName,
                Email = appUser.Email,
                Role = "User",
                CreatedAt = utcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User {Username} registered successfully with ID {UserId}", request.Username, appUser.Id);

            var jwt = await _tokenService.GetTokenByUserId(appUser.Id, staySignedIn: false);
            if (jwt == null)
                return null;

            return new JwtTokenProfile
            {
                Token = jwt.Token,
                ExpiresAtUtc = jwt.ExpirationDate
            };
        }
    }
}
