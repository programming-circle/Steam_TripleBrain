using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Query
{
    public record GetTokenLogsQuery(string Username) : IRequest<IEnumerable<TokenLogs>>; // Запит для отримання логів токенів за ім'ям користувача, повертає колекцію об'єктів TokenLogs
}
