using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.DLCs;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.DLCs
{
    public class GetAllDLCsHandler : IRequestHandler<GetAllDLCsCommand, Result<List<DLC>>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetAllDLCsHandler> _logger;

        public GetAllDLCsHandler(AppDbContext context, ILogger<GetAllDLCsHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<List<DLC>>> Handle(GetAllDLCsCommand request, CancellationToken cancellationToken)
        {
            var dlcs = await _context.DLCs.Include(d => d.Game).ToListAsync(cancellationToken);
            return Result<List<DLC>>.Success(dlcs);
        }
    }
}
