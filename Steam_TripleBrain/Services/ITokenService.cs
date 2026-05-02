using Steam_TripleBrain.Models;
using System.Security.Claims;

namespace Steam_TripleBrain.Services
{
    public interface ITokenService
    {
        Task<JwtToken?> GetTokenByUserId(Guid userId, bool staySignedIn = false);

        Task<Result<User>> DecodeToken(string token, bool checkAdminRole);

        Task<bool> DeleteTokensIfExpired(Guid userId);

        ClaimsPrincipal? ValidateToken(string token);

        Task<Result<User>> DecodeTokenFromHeaders(bool checkAdminRole = false);
    }
}
