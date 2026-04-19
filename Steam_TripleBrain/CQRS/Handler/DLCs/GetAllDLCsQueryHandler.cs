using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Query.DLCs;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.DLCs
{
    public class GetAllDLCsQueryHandler : IRequestHandler<GetAllDLCsQuery, Result<List<DLCViewProfile>>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetAllDLCsQueryHandler> _logger;

        public GetAllDLCsQueryHandler(AppDbContext context, ILogger<GetAllDLCsQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<List<DLCViewProfile>>> Handle(GetAllDLCsQuery request, CancellationToken cancellationToken)
        {
            var dlcs = await _context.DLCs.Include(d => d.Game).ToListAsync(cancellationToken);
            var profiles = dlcs.Select(d => MappingProfile.ToProfile(d)).ToList();
            return Result<List<DLCViewProfile>>.Success(profiles);
        }
    }
}
