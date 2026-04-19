using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Users;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Handler.Users
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersCommand, Result<List<User>>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetAllUsersHandler> _logger;

        public GetAllUsersHandler(AppDbContext context, ILogger<GetAllUsersHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<List<User>>> Handle(GetAllUsersCommand request, CancellationToken cancellationToken)
        {
            var users = await _context.Users.Include(u => u.PurchasedGames).ToListAsync(cancellationToken);
            return Result<List<User>>.Success(users);
        }
    }
}
