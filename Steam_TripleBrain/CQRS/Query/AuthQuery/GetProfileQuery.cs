using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Query.AuthQuery
{
    public record GetProfileQuery(string Username) : IRequest<ProfilesAcc>; // Запит для отримання профілю за ім'ям користувача, повертає об'єкт ProfilesAcc
}
