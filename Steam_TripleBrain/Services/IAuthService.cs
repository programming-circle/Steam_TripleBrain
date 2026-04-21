using System.Threading.Tasks;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(string email, string password); // Register using identity AppUser
        Task<string?> LoginAsync(string username, string password); // Метод для входу користувача, який повертає JWT токен при успішній аутентифікації

    }
}
