using MediatR;

namespace Steam_TripleBrain.CQRS.Command
{
    public record LoginProfileCommand(string Username, string Password) : IRequest<string>; // Запит для входу в профіль, приймає ім'я користувача та пароль, повертає токен у вигляді рядка
}
