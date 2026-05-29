using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.DLCs;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.DLCs
{
    public class UpdateDLCHandler : IRequestHandler<UpdateDLCCommand, Result<DLCViewProfile>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateDLCHandler> _logger;

        public UpdateDLCHandler(AppDbContext context, ILogger<UpdateDLCHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<DLCViewProfile>> Handle(UpdateDLCCommand request, CancellationToken cancellationToken)
        {
            var existing = await _context.Games.FirstOrDefaultAsync(g => g.Id == request.Id && g.IsDLC, cancellationToken);
            if (existing == null)
                return Result<DLCViewProfile>.Failure("DLC not found");

            // Update allowed fields
            existing.Name = request.Name;
            existing.Price = request.Price;
            existing.Discount = request.Discount;
            existing.Description = request.Description;

            // If parent game changed, validate it
            if (request.GameId != Guid.Empty && request.GameId != existing.ParentGameId)
            {
                var parent = await _context.Games.FirstOrDefaultAsync(g => g.Id == request.GameId, cancellationToken);
                if (parent == null || parent.IsDLC)
                    return Result<DLCViewProfile>.Failure("Invalid parent game");

                existing.ParentGameId = parent.Id;
            }

            await _context.SaveChangesAsync(cancellationToken);

            var profile = DLCMappingProfile.ToProfile(existing);
            return Result<DLCViewProfile>.Success(profile);
        }
    }
}
