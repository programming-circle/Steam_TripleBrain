using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Repositories.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Steam_TripleBrain.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) => _context = context;

        public async Task AddAsync(ProfilesAcc profile)
        {
            _context.Profiles.Add(profile);                             // Додаємо новий профіль до контексту
            await _context.SaveChangesAsync();
        }

        public async Task<ProfilesAcc> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;                                            // Якщо ім'я користувача порожнє або null, повертаємо null

            var user = await _context.Profiles
                .FirstOrDefaultAsync(u => u.Username == username);      // Використовуємо FirstOrDefaultAsync для пошуку користувача за ім'ям

            return user;                                                // Повертаємо знайденого користувача або null, якщо не знайдено

        }
        public async Task<bool> DeleteAsync(string username)
        {
            var user = await _context.Profiles.FirstOrDefaultAsync(u => u.Username == username); // Знаходимо користувача за ім'ям
            if (user == null) return false;

            _context.Profiles.Remove(user);                                                      // Видаляємо знайденого користувача з бази даних
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ProfilesAcc>> GetAllAsync()
        {
            return await _context.Profiles.ToListAsync();                                        // Повертаємо всі профілі у вигляді списку
        }


    }
}
