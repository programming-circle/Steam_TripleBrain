using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Query.WishLists;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
namespace Steam_TripleBrain.CQRS.Handler.WishLists
{
    public class GetWishListByIdHandler : IRequestHandler<GetWishListByIdQuery, Result<Profiles.WishListViewProfile>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetWishListByIdHandler> _logger;

        public GetWishListByIdHandler(AppDbContext context, ILogger<GetWishListByIdHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Profiles.WishListViewProfile>> Handle(GetWishListByIdQuery request, CancellationToken cancellationToken)
        {
            var list = await _context.WishLists.Include(w => w.WishGames).FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);
            if (list == null)
                return Result<Profiles.WishListViewProfile>.Failure("WishList not found");

            var profile = MappingProfile.ToProfile(list);
            return Result<Profiles.WishListViewProfile>.Success(profile);
        }
    }
}
