using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.DLCs;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.DLCs
{
    public class UpdateDLCHandler : IRequestHandler<UpdateDLCCommand, Result<DLC>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateDLCHandler> _logger;

        public UpdateDLCHandler(AppDbContext context, ILogger<UpdateDLCHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<DLC>> Handle(UpdateDLCCommand request, CancellationToken cancellationToken)
        {
            var existing = await _context.DLCs.Include(d => d.Game).FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);
            if (existing == null)
                return Result<DLC>.Failure("DLC not found");

            // Use mapping profile to create updated entity and apply changes
            var updated = MappingProfile.ToDLC(request);
            updated.Game = await _context.Games.FirstOrDefaultAsync(g => g.Id == request.GameId, cancellationToken);

            updated.Id = existing.Id; // ensure id remains the same
            _context.Entry(existing).CurrentValues.SetValues(updated);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<DLC>.Success(existing);
        }
    }
}
