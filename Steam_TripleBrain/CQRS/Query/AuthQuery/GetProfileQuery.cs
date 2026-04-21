using MediatR;
using Steam_TripleBrain.Data;

namespace Steam_TripleBrain.CQRS.Query.AuthQuery
{
    public record GetProfileQuery(string Username) : IRequest<AppUser>; // Запит для отримання профілю за ім'ям користувача, повертає AppUser
}
