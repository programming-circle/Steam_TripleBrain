using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.DLCs;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Profiles;
using System.Linq;

namespace Steam_TripleBrain.CQRS.Handler.DLCs
{
    public class GetAllDLCsHandler : IRequestHandler<GetAllDLCsCommand, Result<List<DLCViewProfile>>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetAllDLCsHandler> _logger;

        public GetAllDLCsHandler(AppDbContext context, ILogger<GetAllDLCsHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<List<DLCViewProfile>>> Handle(GetAllDLCsCommand request, CancellationToken cancellationToken)
        {
            var dlcGames = await _context.Games.Where(g => g.IsDLC).ToListAsync(cancellationToken);
            var profiles = dlcGames.Select(g => DLCMappingProfile.ToProfile(g)).ToList();
            return Result<List<DLCViewProfile>>.Success(profiles);
        }
    }
}
