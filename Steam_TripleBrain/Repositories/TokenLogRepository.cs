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
        private readonly ILogger<TokenLogRepository> _logger;
        public TokenLogRepository(AppDbContext context, ILogger<TokenLogRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(TokenLogs log)
        {
            _logger.LogInformation("#### Adding async token to Logs in DB and saving");
            _context.TokenLogs.Add(log); //Logging
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TokenLogs>> GetByUserAsync(string username)
        {
            _logger.LogInformation("#### Start GetByUserAsync.");
            if (string.IsNullOrWhiteSpace(username))
            {
                _logger.LogInformation("#### GetByUserAsync: Username is Null!");
                return Enumerable.Empty<TokenLogs>();
                
            }

            var logs = await _context.TokenLogs
                .Where(t => t.Username == username)
                .ToListAsync();

            if (logs.Any())
            {
                _logger.LogInformation("#### GetByUserAsync: Logs empty.");
                return logs;
            }
            else return Enumerable.Empty<TokenLogs>();
        }

        //Getting Token by Async in Log
        public async Task<TokenLogs> GetByTokenAsync(string token)
        {
            _logger.LogInformation("#### Start of Work Get By Token Async");
            if (string.IsNullOrWhiteSpace(token))
            {
                _logger.LogInformation("#### Get By Token Async: token is empty.");
                return null;
            }

            var log = await _context.TokenLogs
                .FirstOrDefaultAsync(t => t.Token == token);
            if (log == null)
            {
                _logger.LogInformation("#### GetByTokenAsync: log is empty");
                return null;
            }
            return log; 

        }

        public async Task RevokeAsync(string token)
        {
            _logger.LogInformation("#### RevokeAsync start work");
            var log = await GetByTokenAsync(token);
            if (log != null)
            {
                log.IsRevoked = true; // Встановлюємо прапорець IsRevoked в true
                await _context.SaveChangesAsync(); // Зберігаємо зміни в базі даних
            }
            else _logger.LogInformation("#### log is empty");
        }
    }
}
