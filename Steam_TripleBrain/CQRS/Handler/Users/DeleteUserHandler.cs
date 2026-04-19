using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Users;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Handler.Users
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Result<bool>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteUserHandler> _logger;

        public DeleteUserHandler(AppDbContext context, ILogger<DeleteUserHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
            if (existing == null)
                return Result<bool>.Failure("User not found");

            _context.Users.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
