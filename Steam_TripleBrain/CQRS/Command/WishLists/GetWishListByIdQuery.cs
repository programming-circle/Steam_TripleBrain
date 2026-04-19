using MediatR;
using Steam_TripleBrain.Profiles;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Query.WishLists
{
    public class GetWishListByIdQuery : IRequest<Result<WishListViewProfile>>
    {
        public Guid Id { get; set; }
    }
}
