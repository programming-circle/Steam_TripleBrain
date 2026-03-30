using FluentValidation;
using MediatR;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Query.Game
{
    public class GetGameByIdQueryRequest : IRequest<Result<GameViewProfile>>
    {
        public Guid gameId { get; set; }

    }

    public class GetGameByIdValidator : AbstractValidator<GetGameByIdQueryRequest>
    {
        public GetGameByIdValidator()
        {
            RuleFor(x => x.gameId).NotEmpty();
            //RuleFor(x => x.gameId).Must(x => Guid.TryParse(x, out var result) == true).WithMessage("Incorrect {PropertyName} format");
        }
    }
}
