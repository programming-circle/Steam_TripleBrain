using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Query.WishLists;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Handler.WishLists
{
    public class GetAllWishListsHandler : IRequestHandler<GetAllWishListsQuery, Result<List<Profiles.WishListViewProfile>>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetAllWishListsHandler> _logger;

        public GetAllWishListsHandler(AppDbContext context, ILogger<GetAllWishListsHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<List<Profiles.WishListViewProfile>>> Handle(GetAllWishListsQuery request, CancellationToken cancellationToken)
        {
            var lists = await _context.WishLists.Include(w => w.WishGames).ToListAsync(cancellationToken);
            var profiles = lists.Select(w => MappingProfile.ToProfile(w)).ToList();
            return Result<List<Profiles.WishListViewProfile>>.Success(profiles);
        }
    }
}
