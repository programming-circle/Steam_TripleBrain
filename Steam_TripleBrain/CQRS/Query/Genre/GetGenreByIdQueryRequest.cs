using FluentValidation;
using MediatR;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Query.Genre
{
    public class GetGenreByIdQueryRequest : IRequest<Result<GenreViewProfile>>
    {
        public Guid Id { get; set; }
    }

    public class GetGenreByIdValidator : AbstractValidator<GetGenreByIdQueryRequest>
    {
        public GetGenreByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            //RuleFor(x => x.Id).Must(x => Guid.TryParse(x, out var result) == true).WithMessage("Incorrect {PropertyName} format");
        }
    }
}
