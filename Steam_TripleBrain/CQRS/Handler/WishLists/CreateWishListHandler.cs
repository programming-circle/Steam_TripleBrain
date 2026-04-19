using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.WishLists;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.WishLists
{
    public class CreateWishListHandler : IRequestHandler<CreateWishListCommand, Result<WishList>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateWishListHandler> _logger;

        public CreateWishListHandler(AppDbContext context, ILogger<CreateWishListHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<WishList>> Handle(CreateWishListCommand request, CancellationToken cancellationToken)
        {
            var list = MappingProfile.ToWishList(request);

            _context.WishLists.Add(list);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<WishList>.Success(list);
        }
    }
}
