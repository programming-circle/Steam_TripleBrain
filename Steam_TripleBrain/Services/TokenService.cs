using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Steam_TripleBrain.Services
{
    /// <summary>Сервис токенов по образцу amazon-backend: JWT хранится в БД (JwtTokens + TokenJournals), без issuer/audience.</summary>
    public class TokenService : ITokenService
    {
        private readonly string _secretKey;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<TokenService> _logger;

        public TokenService(
            IOptions<Options.Token.TokenOptions> options,
            AppDbContext context,
            IHttpContextAccessor httpContextAccessor,
            ILogger<TokenService> logger)
        {
            options.Value.Validate();
            _secretKey = options.Value.SecretKey;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<JwtToken?> GetTokenByUserId(Guid userId, bool staySignedIn = false)
        {
            User? user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId && u.DeletedAt == null);

            if (user == null)
                return null;

            await DeleteTokensIfExpired(userId);

            TokenJournal? tokenJournal = await _context.TokenJournals
                .Include(tj => tj.Token)
                .AsNoTracking()
                .FirstOrDefaultAsync(tj => tj.UserId == userId && tj.DeactivatedAt == null);

            if (tokenJournal != null)
            {
                if (tokenJournal.Token != null)
                {
                    var validated = ValidateToken(tokenJournal.Token.Token);
                    if (validated != null)
                        return tokenJournal.Token;

                    _context.JwtTokens.Remove(tokenJournal.Token);
                    await _context.SaveChangesAsync();
                }

                _context.TokenJournals.Remove(tokenJournal);
                await _context.SaveChangesAsync();
            }

            return await GenerateToken(userId, staySignedIn);
        }

        private async Task<JwtToken> GenerateToken(Guid userId, bool staySignedIn)
        {
            var claims = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var exp = staySignedIn ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddHours(24);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = exp,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var jwt = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var tokenModel = new JwtToken
            {
                Id = Guid.NewGuid(),
                Token = tokenHandler.WriteToken(jwt),
                ExpirationDate = exp
            };

            var journal = new TokenJournal
            {
                Id = Guid.NewGuid(),
                TokenId = tokenModel.Id,
                UserId = userId,
                ActivatedAt = DateTime.UtcNow
            };

            await _context.JwtTokens.AddAsync(tokenModel);
            await _context.TokenJournals.AddAsync(journal);
            await _context.SaveChangesAsync();

            return tokenModel;
        }

        public async Task<bool> DeleteTokensIfExpired(Guid userId)
        {
            var journals = await _context.TokenJournals
                .Include(tj => tj.Token)
                .Where(tj => tj.UserId == userId)
                .ToListAsync();

            foreach (var tj in journals)
            {
                if (tj.Token != null && DateTime.UtcNow > tj.Token.ExpirationDate)
                    _context.JwtTokens.Remove(tj.Token);
            }

            if (journals.Count > 0)
                await _context.SaveChangesAsync();

            return journals.Count > 0;
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                return tokenHandler.ValidateToken(token, validationParameters, out _);
            }
            catch
            {
                return null;
            }
        }

        public async Task<Result<User>> DecodeTokenFromHeaders(bool checkAdminRole = false)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                _logger.LogError("HttpContext not available");
                return new Result<User>("See server log") { statusCode = 500 };
            }
            string? token = httpContext.Request.Headers["Authorization"];
            if (token != null)
            {
                var result = await DecodeToken(token, checkAdminRole);
                return result;
            }
            return new("Token required") { statusCode = 401 };

            //string? authHeader = httpContext.Request.Headers.Authorization;
            //if (authHeader != null)
            //    return await DecodeToken(authHeader, checkAdminRole);

            //return new Result<User>("Token required") { statusCode = 401 };
        }

        public async Task<Result<User>> DecodeToken(string token, bool checkAdminRole)
        {
            if (string.IsNullOrEmpty(token))
                return new Result<User>("Token required") { statusCode = 401 };

            var index = token.IndexOf(" ");
            if (index < 0)
            {
                return new("Token required") { statusCode = 401 };
            }

            var _token = token.Substring(index + 1);

            JwtToken? jwtToken = await _context.JwtTokens.AsNoTracking()
                .FirstOrDefaultAsync(j => j.Token == _token);

            if (jwtToken == null)
                return new Result<User>("Token rejected") { statusCode = 403 };

            TokenJournal? tj = await _context.TokenJournals
                .Include(t => t.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TokenId == jwtToken.Id);

            if (tj == null)
                return new Result<User>("Token rejected") { statusCode = 403 };

            if (tj.DeactivatedAt != null)
                return new Result<User>("Token rejected") { statusCode = 403 };

            if (tj.User == null)
                return new Result<User>("Forbidden") { statusCode = 403 };

            if (tj.User.DeletedAt != null)
                return new Result<User>("Forbidden") { statusCode = 403 };

            if (checkAdminRole && tj.User.Role != "Admin")
                return new Result<User>("Forbidden") { statusCode = 403 };

            return new Result<User>(tj.User);
        }
    }
}
