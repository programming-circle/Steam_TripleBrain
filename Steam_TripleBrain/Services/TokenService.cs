
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
//using Steam_TripleBrain.Repositories.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Steam_TripleBrain.Services
{
    public class TokenService : ITokenService
    {
        public readonly IConfiguration _configuration;
        public readonly AppDbContext _context;
        public readonly UserManager<AppUser> _userManager;

        public TokenService(UserManager<AppUser> userManager,
            IConfiguration configuration, AppDbContext dbContext)
        {
            _userManager = userManager;
            _context = dbContext;
            _configuration = configuration;
        }

        public async Task<AccessTokenResult> CreateAccessTokenAsync(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Name, user.UserName ?? "")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var minutes = int.Parse(_configuration["Jwt:AccessTokenMinutes"] ?? "30");
            var expire = DateTime.UtcNow.AddMinutes(minutes);

            var jwt = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expire,
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);

            return await Task.FromResult(new AccessTokenResult
            {
                Token = tokenString,
                ExpiresAtUtc = expire
            });
        }

        public async Task<RefreshTokenResult> CreateRefreshTokenAsync(AppUser user)
        {

            var bytes = RandomNumberGenerator.GetBytes(32);
            var refreshToken = Convert.ToBase64String(bytes);

            var days = int.Parse(_configuration["Jwt:RefreshTokenDays"] ?? "7");
            var expires = DateTime.UtcNow.AddDays(days);

            var entity = new RefreshToken
            {
                Token = refreshToken,
                ExpireAtUtc = expires,
                IsRevoke = false,
                UserId = user.Id
            };

            _context.RefreshTokens.Add(entity);
            await _context.SaveChangesAsync();

            return new RefreshTokenResult
            {
                Token = refreshToken,
                ExpireAtUtc = expires
            };
        }

        public async Task<AuthResponse?> RefreshAsync(string refreshToken)
        {
            var tokenEntity = await _context.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (tokenEntity == null)
                return null;

            if (tokenEntity.IsRevoke)
                return null;

            if (tokenEntity.ExpireAtUtc <= DateTime.UtcNow) return null;

            var user = tokenEntity.User;

            var access = await CreateAccessTokenAsync(user);
            var refresh = await CreateRefreshTokenAsync(user);

            await _context.SaveChangesAsync();
            return new AuthResponse
            {
                Accesstoken = access.Token,
                AccessExpiresAtUtc = access.ExpiresAtUtc,
                RefreshToken = refresh.Token,
                RefreshExpiresAtUtc = refresh.ExpireAtUtc,
            };
        }

        public async Task<bool> RevokeAsync(string refreshToken)
        {
            var tokenEnitity = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (tokenEnitity == null) return false;

            tokenEnitity.IsRevoke = true;

            await _context.SaveChangesAsync();
            return true;

        }
    }

}
