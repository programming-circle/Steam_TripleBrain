using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.DLCs;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.DLCs
{
    public class CreateDLCHandler : IRequestHandler<CreateDLCCommand, Result<DLC>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateDLCHandler> _logger;

        public CreateDLCHandler(AppDbContext context, ILogger<CreateDLCHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<DLC>> Handle(CreateDLCCommand request, CancellationToken cancellationToken)
        {
            // Map using mapping profile; resolve Game separately
            var dlc = MappingProfile.ToDLC(request);
            dlc.Game = await _context.Games.FirstOrDefaultAsync(g => g.Id == request.GameId, cancellationToken);

            _context.DLCs.Add(dlc);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<DLC>.Success(dlc);
        }
    }
}
