using FluentValidation;
using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.Auth
{
    public class RegisterCommand : IRequest<AuthResponse>
    {
        public Guid Id { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator() {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
            RuleFor(x => x.Username).MinimumLength(3).WithMessage("Username must be at least 3 characters long.");
            RuleFor(x => x.Username).MaximumLength(20).WithMessage("Username can't be longer 20 characters.");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Not valid Email adress");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Email is required.");
            RuleFor(x => x.Password).MinimumLength(3).WithMessage("Password must be at least 3 characters long.");
            RuleFor(x => x.Password).MaximumLength(20).WithMessage("Password can't be longer 20 characters.");

        }
    }
}
