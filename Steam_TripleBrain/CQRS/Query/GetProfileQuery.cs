using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Query
{
    public record GetProfileQuery(string Username) : IRequest<Profiles>; // Запит для отримання профілю за ім'ям користувача, повертає об'єкт Profiles
}
