
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles.Tokens;

//using Steam_TripleBrain.Repositories.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Steam_TripleBrain.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _secretKey;
        public readonly IConfiguration _configuration;
        public readonly AppDbContext _context;
        public readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpCA;
        public readonly ILogger<TokenService> _logger;

        public TokenService(IOptions<Options.Token.TokenOptions> options,
            UserManager<AppUser> userManager,
            IConfiguration configuration, AppDbContext dbContext,
            ILogger<TokenService> logger, IHttpContextAccessor httpCA)
        {
            options.Value.Validate();
            _secretKey = options.Value.SecretKey;
            _userManager = userManager;
            _context = dbContext;
            _configuration = configuration;
            _logger = logger;
            _httpCA = httpCA;
        }

        private async Task<JwtToken> GenerateToken(Guid userId, bool staySignedIn)
        {
            _logger.LogInformation("Generating Token for user {userId}", userId);
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

            var newToken = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var tokenModel = new JwtToken
            {
                Id = Guid.NewGuid(),
                Token = tokenHandler.WriteToken(newToken),
                ExpirationDate = exp
            };
            var tokenJournal = new TokenJournal
            {
                Id = Guid.NewGuid(),
                TokenId = tokenModel.Id,
                UserId = userId,
                ActivatedAt = DateTime.Now

            };
            await _context.JwtTokens.AddAsync(tokenModel);
            await _context.TokenJournals.AddAsync(tokenJournal);
            await _context.SaveChangesAsync();
            return tokenModel;
        }

        //public async Task<AccessTokenResult> CreateAccessTokenAsync(AppUser user)
        //{
        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //        new Claim(ClaimTypes.Email, user.Email ?? ""),
        //        new Claim(ClaimTypes.Name, user.UserName ?? "")
        //    };

        //    var key = new SymmetricSecurityKey(
        //        Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        //    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var minutes = int.Parse(_configuration["Jwt:AccessTokenMinutes"] ?? "30");
        //    var expire = DateTime.UtcNow.AddMinutes(minutes);

        //    var jwt = new JwtSecurityToken(
        //        issuer: _configuration["Jwt:Issuer"],
        //        audience: _configuration["Jwt:Audience"],
        //        claims: claims,
        //        expires: expire,
        //        signingCredentials: credentials
        //    );

        //    var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);

        //    return await Task.FromResult(new AccessTokenResult
        //    {
        //        Token = tokenString,
        //        ExpiresAtUtc = expire
        //    });
        //}

        //public async Task<RefreshTokenResult> CreateRefreshTokenAsync(AppUser user)
        //{

        //    var bytes = RandomNumberGenerator.GetBytes(32);
        //    var refreshToken = Convert.ToBase64String(bytes);

        //    var days = int.Parse(_configuration["Jwt:RefreshTokenDays"] ?? "7");
        //    var expires = DateTime.UtcNow.AddDays(days);

        //    var entity = new RefreshToken
        //    {
        //        Token = refreshToken,
        //        ExpireAtUtc = expires,
        //        IsRevoke = false,
        //        UserId = user.Id
        //    };

        //    _context.RefreshTokens.Add(entity);
        //    await _context.SaveChangesAsync();

        //    return new RefreshTokenResult
        //    {
        //        Token = refreshToken,
        //        ExpireAtUtc = expires
        //    };
        //}

        /*public async Task<JwtToken?> RefreshAsync(string refreshToken)
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
            return new JwtToken
            {
                Accesstoken = access.Token,
                AccessExpiresAtUtc = access.ExpiresAtUtc,
                RefreshToken = refresh.Token,
                RefreshExpiresAtUtc = refresh.ExpireAtUtc,
            };
        }*/
        /*
        public async Task<bool> RevokeAsync(string refreshToken)
        {
            var tokenEnitity = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (tokenEnitity == null) return false;

            tokenEnitity.IsRevoke = true;

            await _context.SaveChangesAsync();
            return true;

        }
        */
        public async Task<Result<User>> DecodeToken(string token, /*bool checkAdminRole*/)
        {
            if (string.IsNullOrEmpty(token))
            {
                return new("Token required") { statusCode = 401 };
            }
            var index = token.IndexOf(" ");
            if (index < 0)
            {
                return new("Token required") { statusCode = 401 };
            }
            var _token = token.Substring(index + 1);
            JwtToken? jwtToken = await _context.JwtTokens.AsNoTracking().FirstOrDefaultAsync(j => j.Token == _token);
            if (jwtToken == null)
            {
                return new("Token rejected") { statusCode = 403 };
            }
            TokenJournal? tj = await _context.TokenJournals.Include(t => t.User).AsNoTracking().FirstOrDefaultAsync(t => t.TokenId == jwtToken.Id);
            if (tj == null)
            {
                return new("Token rejected") { statusCode = 403 };
            }
            if (tj.DeactivatedAt != null)
            {
                return new("Token rejected") { statusCode = 403 };
            }
            if (tj.User != null)
            {
                if (tj.User.DeletedAt != null)
                {
                    return new("Forbidden") { statusCode = 403 };
                }
                if (checkAdminRole && tj.User.Role != "Admin")
                {
                    return new("Forbidden") { statusCode = 403 };
                }
                return new(tj.User);
            }
            return new("Forbidden") { statusCode = 403 };
        }

        public async Task<JwtToken?> GetTokenByUserId(Guid Id, bool staySignedIn = false)
        {
            _logger.LogInformation("Getting Token by UserId {Id}", Id);
            User? user = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == Id && x.DeletedAt == null);

            if (user == null )
            {
                _logger.LogInformation("### GetTokenByUserId: user {user} is null", user);
                return null;
            }
            _logger.LogInformation("### GetTokenByUserId: Checking if token expired and deleting it.");
            await DeleteTokensIfExpired(Id);
            _logger.LogInformation("### GetTokenByUserId: searching for user by Id");
            TokenJournal? tokenJournal = await _context.TokenJournals.Include(t => t.Token)
                .AsNoTracking().FirstOrDefaultAsync(t => t.UserId == Id && t.DeactivatedAt == null); ;
            if(tokenJournal != null)
            {
                if(tokenJournal.Token != null)
                {
                    string token = tokenJournal.Token.Token;
                    var result = ValidateToken(token);
                    if(result != null)
                    {
                        _logger.LogInformation("### GetTokenByUserId: Token successfully get ");
                        return tokenJournal.Token;
                    }
                    _logger.LogInformation("### GetTokenByUserId: result is null , deleting token of journal");
                    _context.Remove(tokenJournal);
                    await _context.SaveChangesAsync();
                }
                _context.Remove(tokenJournal);
                await _context.SaveChangesAsync();
            }
            var newToken = await GenerateToken(Id, staySignedIn);
            return newToken;
        }


        public async Task<Result<User>> DecodeToken(string token, bool checkAdminRole)
        {
            if(string.IsNullOrEmpty(token))
            {
                return new("Token required") { statusCode = 401 };
            }
            var index = token.IndexOf(" ");
            if (index < 0 )
            {
                return new("Token required") { statusCode = 401 };
            }
            var _token = token.Substring(index + 1);
            JwtToken? jwtToken = await _context.JwtTokens.AsNoTracking().FirstOrDefaultAsync(j => j.Token == _token);
            if (jwtToken == null)
            {
                return new("Token rejected") { statusCode = 403 };
            }
            TokenJournal? tj = await _context.TokenJournals.Include(t => t.User).AsNoTracking().FirstOrDefaultAsync(t => t.TokenId == jwtToken.Id);
            if (tj == null)
            {
                return new("Token rejected") { statusCode = 403 };
            }
            if (tj.DeactivatedAt != null)
            {
                return new("Token rejected") { statusCode = 403 };
            }
            if (tj.User != null)
            {
                if (tj.User.DeletedAt != null)
                {
                    return new("Forbidden") { statusCode = 403 };
                }
                if (checkAdminRole && tj.User.Role != "Admin")
                {
                    return new("Forbidden") { statusCode = 403 };
                }
                return new(tj.User);
            }
            return new("Forbidden") { statusCode = 403 };
        }

        public async Task<bool> DeleteTokensIfExpired(Guid userId)
        {
            _logger.LogInformation("Deleting Token if it's expired");
            List<TokenJournal>? tokenJournal = await _context.TokenJournals
                .Include(t => t.Token).Where(t => t.UserId == userId).ToListAsync();

            if(tokenJournal != null)
            {
                foreach(var t in tokenJournal)
                {
                    if (t.Token != null)
                    {
                        if(DateTime.Now > t.Token.ExpirationDate)
                        {
                            _logger.LogInformation("### DeletingToken: deleting token cause it's expired");
                            _context.JwtTokens.Remove(t.Token);
                            await _context.SaveChangesAsync();
                        }
                    }
                    
                }
                _logger.LogInformation("### DeletingToken: tokens updated , all expired removed");
                return true;
            }
            _logger.LogInformation("### DeletingToken: Journal with tokens is empty");
            return false;

        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            _logger.LogInformation("Validate Token start work");
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
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }
            catch (Exception exception)
            {
                _logger.LogError("### ValidateToken: exception caused: {exception}", exception);
                return null;
            }
        }

        public async Task<Result<User>> DecodeTokenFromHeaders(bool checkAdminRole = false)
        {
            _logger.LogInformation("Decode Token from headers started");
            var httpContext = _httpCA.HttpContext;
            if (httpContext == null)
            {
                _logger.LogError("HttpContext not avaliable");
                return new("See server log") { statusCode = 500 };
            }
            string? token = httpContext.Request.Headers["Authorization"];
            if (token != null)
            {
                var result = await DecodeToken(token, checkAdminRole);
                return result;
            }
            return new("Token required") { statusCode = 401 };
        }

        
    }
