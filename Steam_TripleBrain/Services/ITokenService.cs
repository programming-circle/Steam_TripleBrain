
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using System.Security.Claims;

namespace Steam_TripleBrain.Services
{
    public interface ITokenService
    {
        //string GenerateToken(ProfilesAcc user); //Generate Token JWT
        //Task<AccessTokenResult> CreateAccessTokenAsync(AppUser user);
        //Task<AccessTokenResult> CreateAccessTokenAsync(ProfilesAcc user);
        //Task<RefreshTokenResult> CreateRefreshTokenAsync(AppUser user);
        Task<JwtToken?> GetTokenByUserId(Guid Id, bool staySignedIn = false);
        Task<JwtToken> GenerateToken(Guid userIdbool , bool staySignedIn);
        //Task<JwtToken?> RefreshAsync(string refreshToken);
        //Task<bool> RevokeAsync(string refreshToken);
        Task<Result<User>> DecodeToken(string token, bool checkAdminRole);

        Task<bool> DeleteTokensIfExpired(Guid userId);
        ClaimsPrincipal? ValidateToken(string token);

        Task<Result<User>> DecodeTokenFromHeaders(bool checkAdminRole = false);
    }
}
