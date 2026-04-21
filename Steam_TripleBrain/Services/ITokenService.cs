
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.Services
{
    public interface ITokenService
    {
        //string GenerateToken(ProfilesAcc user); //Generate Token JWT
        Task<AccessTokenResult> CreateAccessTokenAsync(AppUser user);
        //Task<AccessTokenResult> CreateAccessTokenAsync(ProfilesAcc user);
        Task<RefreshTokenResult> CreateRefreshTokenAsync(AppUser user);
        Task<AuthResponse?> RefreshAsync(string refreshToken);
        Task<bool> RevokeAsync(string refreshToken);
    }
}
