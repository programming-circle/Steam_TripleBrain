using System.Threading.Tasks;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.Services.Interface
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(Profiles profile); // Метод для реєстрації нового користувача
        Task<string?> LoginAsync(string username, string password); // Метод для входу користувача, який повертає JWT токен при успішній аутентифікації

    }
}
