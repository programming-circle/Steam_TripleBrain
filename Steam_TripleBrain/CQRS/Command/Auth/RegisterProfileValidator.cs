using FluentValidation;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.Auth
{
    public class RegisterProfileValidator : AbstractValidator<RegisterProfileCommand>
    {
        public RegisterProfileValidator()
        {
            RuleFor(x => x.Profile).NotNull().WithMessage("Profile is required.");

            When(x => x.Profile != null, () =>
            {
                RuleFor(x => x.Profile.Username)
                    .NotEmpty().WithMessage("Username is required.")
                    .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
                    .MaximumLength(50).WithMessage("Username must not exceed 50 characters.");

                RuleFor(x => x.Profile.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                    .MaximumLength(100).WithMessage("Password must not exceed 100 characters.");

                RuleFor(x => x.Profile.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email format.")
                    .MaximumLength(200).WithMessage("Email must not exceed 200 characters.");

                RuleFor(x => x.Profile.FullName)
                    .MaximumLength(200).WithMessage("Full name must not exceed 200 characters.");

                RuleFor(x => x.Profile.Role)
                    .NotEmpty().WithMessage("Role is required.");
            });
        }
    }
}
