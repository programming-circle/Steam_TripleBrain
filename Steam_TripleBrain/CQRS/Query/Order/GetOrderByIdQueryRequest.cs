using FluentValidation;
using MediatR;
using Steam_TripleBrain.CQRS.Query.Game;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Query.Order
{
    public class GetOrderByIdQueryRequest : IRequest<Result<OrderViewProfile>>
    {
        public Guid Id { get; set; }

    }
    public class GetOrderByIdValidator : AbstractValidator<GetOrderByIdQueryRequest>
    {
        public GetOrderByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            //RuleFor(x => x.Id).Must(x => Guid.TryParse(x, out var result) == true).WithMessage("Incorrect {PropertyName} format");
        }
    }
}
