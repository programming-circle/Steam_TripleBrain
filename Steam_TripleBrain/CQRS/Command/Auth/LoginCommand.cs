using FluentValidation;
using MediatR;
using Steam_TripleBrain.Profiles.Tokens;

namespace Steam_TripleBrain.CQRS.Command.Auth
{
    public class LoginCommand : IRequest<JwtTokenProfile?>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool StaySignedIn { get; set; }
    }

    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }
}
