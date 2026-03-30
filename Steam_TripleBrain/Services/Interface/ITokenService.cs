using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.Services.Interface
{
    public interface ITokenService
    {
        string GenerateToken(ProfilesAcc user); // Генерация JWT токена на основі информації про користувача

    }
}
