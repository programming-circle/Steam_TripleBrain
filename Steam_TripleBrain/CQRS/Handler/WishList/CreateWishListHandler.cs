using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Game;
using Steam_TripleBrain.CQRS.Command.WishList;
using Steam_TripleBrain.CQRS.Handler.Game;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.WishList
{
    public class CreateWishListHandler : IRequestHandler<CreateWishListCommand, Result<WishListViewProfile>>
    {

        private readonly AppDbContext _context;
        private readonly ILogger<CreateWishListHandler> _logger;


        public CreateWishListHandler(AppDbContext context, ILogger<CreateWishListHandler> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<Result<WishListViewProfile>> Handle(CreateWishListCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateWishlistHandler for wishlist with User {UserId}", request.UserId);
            var exists = await _context.Games.AnyAsync(g => g.Id == request.Id, cancellationToken);
            _logger.LogInformation("Checking if game with ID {GameId} exists: {Exists}", request.Id, exists);

            if (exists)
            {   //Return failure result if game already exists
                _logger.LogWarning("### CreateWishlistHandler: WishList for UserID {UserId} already exists. Cannot create.", request.UserId);
                return Result<WishListViewProfile>.Failure($"### CreateWishlistHandler: WishList for UserID {request.UserId} already exists. Cannot create.");
            }

            var wishlist = WishListMappingProfile.ToWishList(request);
            
            await _context.AddAsync(wishlist);
            await _context.SaveChangesAsync();
            _logger.LogInformation("### CreateWishlistHandler: Successfuly added new WishList to DB ");

            var wishlistProfile = WishListMappingProfile.ToProfile(wishlist);

            return Result<WishListViewProfile>.Success(wishlistProfile);
        }
    }
}
