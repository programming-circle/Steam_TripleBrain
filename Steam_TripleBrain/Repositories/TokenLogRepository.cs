using System.Linq;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Repositories.Interface;

namespace Steam_TripleBrain.Repositories
{
    public class TokenLogRepository : ITokenLogRepository
    {
        private readonly AppDbContext _context;
        public TokenLogRepository(AppDbContext context)
        {
            _context = context; 
        }

        public async Task AddAsync(TokenLogs log)
        {
            _context.TokenLogs.Add(log); // Додаємо новий лог до контексту бази даних
            await _context.SaveChangesAsync(); // Зберігаємо зміни в базі даних після додавання нового лога
        }

        public async Task<IEnumerable<TokenLogs>> GetByUserAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return Enumerable.Empty<TokenLogs>(); // Якщо ім'я користувача порожнє або null, повертаємо порожню колекцію

            var logs = await _context.TokenLogs
                .Where(t => t.Username == username)
                .ToListAsync(); // Шукаємо всі логи, пов'язані з вказаним ім'ям користувача

            return logs ?? Enumerable.Empty<TokenLogs>(); // Повертаємо знайдені логи або порожню колекцію, якщо не знайдено
        }

        public async Task<TokenLogs> GetByTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null; // Якщо токе порожній або null, повертаємо null

            var log = await _context.TokenLogs
                .FirstOrDefaultAsync(t => t.Token == token); // Шукаємо токен у базі даних

            return log; // Повертаємо знайдений лог або null, якщо не знайдено

        }

        public async Task RevokeAsync(string token)
        {
            var log = await GetByTokenAsync(token); // Використовуємо метод GetByTokenAsync для отримання логів за токеном
            if (log != null)
            {
                log.IsRevoked = true; // Встановлюємо прапорець IsRevoked в true
                await _context.SaveChangesAsync(); // Зберігаємо зміни в базі даних
            }
        }
    }
}
