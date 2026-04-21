using Microsoft.AspNetCore.Identity;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.Data
{
    public class AppUser : IdentityUser<Guid>
    {
        public List<RefreshToken> RefreshTokens { get; set; } = new();
    }
}
