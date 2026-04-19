using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Users;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Handler.Users
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdCommand, Result<User>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetUserByIdHandler> _logger;

        public GetUserByIdHandler(AppDbContext context, ILogger<GetUserByIdHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<User>> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Include(u => u.PurchasedGames).FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
            if (user == null)
                return Result<User>.Failure("User not found");

            return Result<User>.Success(user);
        }
    }
}
