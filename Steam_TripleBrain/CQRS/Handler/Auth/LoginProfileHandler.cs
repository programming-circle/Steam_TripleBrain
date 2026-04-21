using MediatR;
using Steam_TripleBrain.CQRS.Command;
using Steam_TripleBrain.Services;

namespace Steam_TripleBrain.CQRS.Handler.Auth
{
    public class LoginProfileHandler : IRequestHandler<LoginProfileCommand, string>
    {
        private readonly IAuthService _authService;
        public LoginProfileHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<string> Handle(LoginProfileCommand request, CancellationToken cancellationToken)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Username and password are required");          // Якщо дані не валідні — кидаємо виняток або повертаємо null

            var token = await _authService.LoginAsync(request.Username, request.Password);  // Викликаємо сервіс аутентифікації для отримання токена

            if (token == null)
                return null;                                                                // Якщо аутентифікація не вдалася, повертаємо null

            return token;                                                                   // Повертаємо отриманий токен

        }
    }
}
