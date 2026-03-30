using Steam_TripleBrain.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Steam_TripleBrain.Repositories.Interface
{
    public interface IUserRepository
    {
        Task AddAsync(Profiles profile);                    // Додайте новый профиль
        Task<Profiles> GetByUsernameAsync(string username); // Получите профиль по имени пользователя
        Task<bool> DeleteAsync(string username);
        Task<IEnumerable<Profiles>> GetAllAsync();
    }
}
