using MediatR;
using Steam_TripleBrain.CQRS.Command.Auth;
using Steam_TripleBrain.Services;

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
            if (request == null)
                throw new ArgumentException("Registration data is required");

            var result = await _authService.RegisterAsync(request.Email, request.Password);

            return result;                                                      // Повертаємо результат реєстрації (true або false)
        }
    }
}

