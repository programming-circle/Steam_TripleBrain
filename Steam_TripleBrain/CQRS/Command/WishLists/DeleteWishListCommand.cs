using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.WishLists
{
    public class DeleteWishListCommand : IRequest<Result<bool>>
    {
        public Guid Id { get; set; }
    }
}
