using FluentValidation;
using MediatR;
using Steam_TripleBrain.CQRS.Query.Order;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Query.WishList
{
    public class GetByUserIdWishListQuery : IRequest<Result<WishListViewProfile>>
    {
        public Guid Id { get; set; }
    }

    public class GetByUserIdWishListValidator : AbstractValidator<GetByUserIdWishListQuery>
    {
        public GetByUserIdWishListValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            
        }
    }
}
