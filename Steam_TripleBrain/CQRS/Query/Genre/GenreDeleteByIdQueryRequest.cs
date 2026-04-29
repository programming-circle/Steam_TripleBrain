using FluentValidation;
using MediatR;
using Steam_TripleBrain.CQRS.Query.Order;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Query.Genre
{
    public class GenreDeleteByIdQueryRequest : IRequest<Result<GenreViewProfile>>
    {
        public Guid Id { get; set; }
    }

    public class GenreDeleteByIdValidator : AbstractValidator<GenreDeleteByIdQueryRequest>
    {
        public GenreDeleteByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            //RuleFor(x => x.Id).Must(x => Guid.TryParse(x, out var result) == true).WithMessage("Incorrect {PropertyName} format");
        }
    }
}
