using Steam_TripleBrain.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Steam_TripleBrain.Repositories.Interface
{
    public interface IUserRepository
    {
        Task AddAsync(ProfilesAcc profile);                    // Додайте новый профиль
        Task<ProfilesAcc> GetByUsernameAsync(string username); // Получите профиль по имени пользователя
        Task<bool> DeleteAsync(string username);
        Task<IEnumerable<ProfilesAcc>> GetAllAsync();
    }
}
