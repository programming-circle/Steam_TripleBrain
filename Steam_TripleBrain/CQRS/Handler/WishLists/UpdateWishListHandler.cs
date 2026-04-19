using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.WishLists;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.WishLists
{
    public class UpdateWishListHandler : IRequestHandler<UpdateWishListCommand, Result<WishList>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateWishListHandler> _logger;

        public UpdateWishListHandler(AppDbContext context, ILogger<UpdateWishListHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<WishList>> Handle(UpdateWishListCommand request, CancellationToken cancellationToken)
        {
            var existing = await _context.WishLists.Include(w => w.WishGames).FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);
            if (existing == null)
                return Result<WishList>.Failure("WishList not found");

            var updated = MappingProfile.ToWishList(request);
            updated.Id = existing.Id;

            _context.Entry(existing).CurrentValues.SetValues(updated);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<WishList>.Success(existing);
        }
    }
}
