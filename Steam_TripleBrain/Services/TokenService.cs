using Microsoft.IdentityModel.Tokens;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Repositories.Interface;
using Steam_TripleBrain.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Steam_TripleBrain.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly ITokenLogRepository _logRepo;

        public TokenService(IConfiguration config, ITokenLogRepository logRepo)
        {
            _config = config;
            _logRepo = logRepo;
        }

        public string GenerateToken(ProfilesAcc user)
        {
            var claims = new[]                                                              // створення claims на основі даних користувача
            {
            new Claim(ClaimTypes.Name, user.Username),                                      // ім'я користувача
            new Claim(ClaimTypes.Role, user.Role)                                           // роль користувача ("Admin", "User" тощо)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])); // отримання секретного ключа з конфігурації
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);         // створення об'єкта для підпису токена

        var token = new JwtSecurityToken(                                               // створення JWT токена
            issuer: _config["Jwt:Issuer"],                                              // вказівка видавця токена
            audience: _config["Jwt:Audience"],                                          // вказівка аудиторії токена
            claims: claims,                                                             // додавання claims до токена
            expires: DateTime.UtcNow.AddDays(2),                                        // встановлення часу життя токена (2 години)
            signingCredentials: creds);                                                 // додавання підпису до токена

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);                      // серіалізація токена у строку

        var log = new TokenLogs                                                         // створення запису про виданий токен
        {
            Username = user.Username,                                                   // збереження імені користувача, для якого видано токен
            Token = jwt,                                                                // збереження самого токена
            ExpiredAt = DateTime.UtcNow.AddHours(2)                                     // встановлення часу, коли токен стане недійсним (2 години від поточного часу)
        };
        _logRepo.AddAsync(log).Wait();                                                  // асинхронне збереження запису про виданий токен у базі даних

        return jwt;                                                                     // повернення згенерованого токена у вигляді строки
        }
    }

}
