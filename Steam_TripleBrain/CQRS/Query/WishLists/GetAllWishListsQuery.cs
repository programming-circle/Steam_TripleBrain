using MediatR;
using System.Collections.Generic;
using Steam_TripleBrain.Profiles;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Query.WishLists
{
    public class GetAllWishListsQuery : IRequest<Result<List<WishListViewProfile>>>
    {
    }
}
