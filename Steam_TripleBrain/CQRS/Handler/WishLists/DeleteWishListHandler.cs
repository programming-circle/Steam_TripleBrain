using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.WishLists;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.WishLists
{
    public class DeleteWishListHandler : IRequestHandler<DeleteWishListCommand, Result<bool>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteWishListHandler> _logger;

        public DeleteWishListHandler(AppDbContext context, ILogger<DeleteWishListHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(DeleteWishListCommand request, CancellationToken cancellationToken)
        {
            var existing = await _context.WishLists.FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);
            if (existing == null)
                return Result<bool>.Failure("WishList not found");

            _context.WishLists.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
