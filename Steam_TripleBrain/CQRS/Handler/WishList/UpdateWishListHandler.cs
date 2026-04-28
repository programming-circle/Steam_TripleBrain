using MediatR;
using Steam_TripleBrain.CQRS.Command.WishList;
using Steam_TripleBrain.Data;
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

        public Task<Result<WishListViewProfile>> Handle(UpdateWishListCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
