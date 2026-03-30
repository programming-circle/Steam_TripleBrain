using MediatR;
using Steam_TripleBrain.Models;
using System.Collections.Generic;

namespace Steam_TripleBrain.CQRS.Query.AuthQuery
{
    public record GetTokenLogsQuery(string Username) : IRequest<IEnumerable<TokenLogs>>; // Запит для отримання логів токенів за ім'ям користувача
}


