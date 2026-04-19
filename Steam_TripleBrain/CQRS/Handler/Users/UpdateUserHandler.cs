using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Users;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.Users
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result<User>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateUserHandler> _logger;

        public UpdateUserHandler(AppDbContext context, ILogger<UpdateUserHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<User>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
            if (existing == null)
                return Result<User>.Failure("User not found");

            // Map incoming command to domain user, preserve registration date and navigation properties
            var updated = MappingProfile.ToUser(request);

            // Preserve fields that shouldn't be overwritten
            updated.Id = existing.Id;
            updated.DateOfReg = existing.DateOfReg;
            updated.Icon = existing.Icon;
            updated.PurchasedGames = existing.PurchasedGames;
            updated.DLCs = existing.DLCs;

            // Apply scalar changes to tracked entity
            _context.Entry(existing).CurrentValues.SetValues(updated);

            await _context.SaveChangesAsync(cancellationToken);

            return Result<User>.Success(existing);
        }
    }
}
