using FluentValidation;
using MediatR;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Command.WishList
{
    public class UpdateWishListCommand : IRequest<Result<WishListViewProfile>>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<GameViewProfile>? WishGames { get; set; }
    }

    public class UpdateWishListValidator : AbstractValidator<UpdateWishListCommand>
    {
        public UpdateWishListValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
