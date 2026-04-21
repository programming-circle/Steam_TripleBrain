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
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(AppDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        } 

        public async Task AddAsync(ProfilesAcc profile)
        {
            _logger.LogInformation("#### AddAsync start working");
            _context.Profiles.Add(profile);                             
            await _context.SaveChangesAsync(); 
        }

        public async Task<ProfilesAcc> GetByUsernameAsync(string username)
        {
            _logger.LogInformation("#### GetByUsernameAsync start working");
            if (string.IsNullOrWhiteSpace(username))
            {
                _logger.LogInformation("#### GetByUsernameAsync: username is empty");
                return null;
            }

            var user = await _context.Profiles
                .FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                _logger.LogInformation("#### GetByUsernameAsync: user by username is empty.");
                return null;
            }

            return user;                                               

        }
        public async Task<bool> DeleteAsync(string username)
        {
            _logger.LogInformation("#### DeleteAsync start working");
            var user = await _context.Profiles.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                _logger.LogInformation("#### DeleteAsync: user empty");
                return false;
            }

            _context.Profiles.Remove(user);                                                     
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ProfilesAcc>> GetAllAsync()
        {
            _logger.LogInformation("#### GetAllAsync startWorking");
            return await _context.Profiles.ToListAsync();                                        // Повертаємо всі профілі у вигляді списку
        }


    }
}
