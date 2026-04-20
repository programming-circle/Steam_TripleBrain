using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.Auth
{
    public record RegisterProfileCommand(ProfilesAcc Profile) : IRequest<bool>; // Запит для реєстрації профілю, приймає об'єкт Profiles, повертає булеве значення, що вказує на успішність реєстрації
}
