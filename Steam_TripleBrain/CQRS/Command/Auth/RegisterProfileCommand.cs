using FluentValidation;
using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.Auth
{
    public record RegisterProfileCommand(string Email, string Password) : IRequest<bool>;
    public class RegisterProfileValidator : AbstractValidator<RegisterProfileCommand>
    {
        public RegisterProfileValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(200).WithMessage("Email must not exceed 200 characters.");

            RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                    .MaximumLength(100).WithMessage("Password must not exceed 100 characters.");
        }
    }
}
