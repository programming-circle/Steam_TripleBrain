using FluentValidation;
using MediatR;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;
using System.ComponentModel.DataAnnotations;

namespace Steam_TripleBrain.CQRS.Command.User
{
    public class UpdateMeRequest : IRequest<Result<UserViewProfile>>
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Icon { get; set; } 
    }

    public class UpdateMeValidator : AbstractValidator<UpdateMeRequest>
    {
        public UpdateMeValidator() {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required");
            RuleFor(x => x.Username).MinimumLength(3).WithMessage("Username must be at least 3 characters long.");
            RuleFor(x => x.Username).MaximumLength(20).WithMessage("Username can't be longer 20 characters.");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Not valid Email adress");

        }
    }
}
