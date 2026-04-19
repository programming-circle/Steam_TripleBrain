using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.WishLists
{
    public class CreateWishListCommand : IRequest<Result<WishList>>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<Models.Game>? WishGames { get; set; }
    }
}
