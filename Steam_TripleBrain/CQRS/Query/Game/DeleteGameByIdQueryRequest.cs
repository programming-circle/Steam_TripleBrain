using FluentValidation;
using MediatR;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Query.Game
{
    public class DeleteGameByIdQueryRequest : IRequest<Result<GameViewProfile>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteGameByIdValidator : AbstractValidator<GetGameByIdQueryRequest>
    {
        public DeleteGameByIdValidator()
        {
            RuleFor(x => x.gameId).NotEmpty();
            //RuleFor(x => x.gameId).Must(x => Guid.TryParse(x, out var result) == true).WithMessage("Incorrect {PropertyName} format");
        }
    }
}
