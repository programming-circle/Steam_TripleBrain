using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.WishList;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.WishList
{
    public class UpdateWishListHandler : IRequestHandler<UpdateWishListCommand, Result<WishListViewProfile>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateWishListHandler> _logger;

        public UpdateWishListHandler(AppDbContext context, ILogger<UpdateWishListHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<WishListViewProfile>> Handle(UpdateWishListCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateWishListHandler for wishlist {WishListId}", request.Id);

            var wishlist = await _context.WishLists
                .Include(w => w.WishGames)
                .FirstOrDefaultAsync(w => w.Id == request.Id && w.UserId == request.UserId, cancellationToken);

            if (wishlist == null)
            {
                _logger.LogWarning("Wishlist with Id {WishListId} not found for user {UserId}", request.Id, request.UserId);
                return Result<WishListViewProfile>.Failure("Wishlist not found.");
            }

            wishlist.WishGames = request.WishGames?.Select(g => new Models.Game
            {
                Id = g.Id,
                Name = g.Name,
                Poster = g.Poster,
                Images = g.Images,
                Rating = g.Rating,
                Description = g.Description,
                Genres = g.Genres?.ToList(),
                Price = g.Price,
                Discount = g.Discount,
                Developer = g.Developer,
            }).ToList();

            _context.WishLists.Update(wishlist);
            await _context.SaveChangesAsync(cancellationToken);

            var profile = WishListMappingProfile.ToProfile(wishlist);
            return Result<WishListViewProfile>.Success(profile);
        }
    }
}
