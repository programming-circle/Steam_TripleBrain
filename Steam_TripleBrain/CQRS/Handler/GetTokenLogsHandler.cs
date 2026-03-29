using MediatR;
using Steam_TripleBrain.CQRS.Query;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Repositories.Interface;

namespace Steam_TripleBrain.CQRS.Handler
{
    public class GetTokenLogsHandler : IRequestHandler<GetTokenLogsQuery, IEnumerable<TokenLogs>>
    {
        private readonly ITokenLogRepository _repo;
        public GetTokenLogsHandler(ITokenLogRepository repo) 
        {
            _repo = repo;
        }

        public async Task<IEnumerable<TokenLogs>> Handle(GetTokenLogsQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
                throw new ArgumentException("Username is required"); // Якщо по запросу не передано ім'я користувача, викидаємо виняток

            var logs = await _repo.GetByUserAsync(request.Username); // Отримуємо логи за ім'ям користувача

            if (logs == null || !logs.Any())
                return Enumerable.Empty<TokenLogs>();                // Якщо логів немає, повертаємо порожню колекцію

            return logs;                                             // Повертаємо знайдені логи
        }
    }
}
