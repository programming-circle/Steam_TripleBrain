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
            var dlc = await _context.DLCs.Include(d => d.Game).FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);
            if (dlc == null)
                return Result<DLCViewProfile>.Failure("DLC not found");

            var profile = MappingProfile.ToProfile(dlc);
            return Result<DLCViewProfile>.Success(profile);
        }
    }
}
