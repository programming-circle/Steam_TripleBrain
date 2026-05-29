using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Query.DLCs;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.DLCs
{
    public class GetDLCByIdQueryHandler : IRequestHandler<GetDLCByIdQuery, Result<DLCViewProfile>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetDLCByIdQueryHandler> _logger;

        public GetDLCByIdQueryHandler(AppDbContext context, ILogger<GetDLCByIdQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<DLCViewProfile>> Handle(GetDLCByIdQuery request, CancellationToken cancellationToken)
        {
            // Find the Game entity that represents a DLC
            var dlcGame = await _context.Games.FirstOrDefaultAsync(g => g.Id == request.Id && g.IsDLC, cancellationToken);
            if (dlcGame == null)
                return Result<DLCViewProfile>.Failure("DLC not found");

            var profile = DLCMappingProfile.ToProfile(dlcGame);
            return Result<DLCViewProfile>.Success(profile);
        }
    }
}
