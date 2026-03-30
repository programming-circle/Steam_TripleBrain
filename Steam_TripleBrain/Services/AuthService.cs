using BCrypt.Net;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Repositories.Interface;
using Steam_TripleBrain.Services.Interface;

namespace Steam_TripleBrain.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repo;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository repo, ITokenService tokenService)
        {
            _repo = repo;
            _tokenService = tokenService;
        }

        public async Task<bool> RegisterAsync(ProfilesAcc profile)
        {
            // Ensure we have a plain password to hash. Accept `Password` from incoming requests
            var plain = profile.Password ?? profile.PasswordHash;
            if (string.IsNullOrWhiteSpace(plain))
                return false; // caller (controller) should return BadRequest

            profile.PasswordHash = BCrypt.Net.BCrypt.HashPassword(plain);  // Хешуємо пароль перед збереженням
            profile.Password = null; // Clear plain password so it is not kept in memory/storage

            await _repo.AddAsync(profile); // Зберігаємо користувача в базі даних
            return true; // Повертаємо успішну реєстрацію
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            var user = await _repo.GetByUsernameAsync(username); // Отримуємо користувача за ім'ям користувача
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) // Перевіряємо, чи існує користувач і чи збігається пароль
                return null;

            return _tokenService.GenerateToken(user); // Генерация JWT токена
        }
    }

}
