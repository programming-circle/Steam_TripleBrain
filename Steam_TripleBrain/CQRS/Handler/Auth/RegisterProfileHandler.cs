using MediatR;
using Steam_TripleBrain.CQRS.Command.Auth;
using Steam_TripleBrain.Services.Interface;

namespace Steam_TripleBrain.CQRS.Handler.Auth
{
    public class RegisterProfileHandler : IRequestHandler<RegisterProfileCommand, bool>
    {
        private readonly IAuthService _authService;
        public RegisterProfileHandler(IAuthService authService)
        {
            _authService = authService;

        }

        public async Task<bool> Handle(RegisterProfileCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.Profile == null)
                throw new ArgumentException("Profile data is required");        // Якщо дані профілю не передані, викидаємо виняток

            var result = await _authService.RegisterAsync(request.Profile);     // Викликаємо метод реєстрації з сервісу аутентифікації

            return result;                                                      // Повертаємо результат реєстрації (true або false)
        }
    }
}

