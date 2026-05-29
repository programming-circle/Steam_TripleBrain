using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.DLCs;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.DLCs
{
    public class CreateDLCHandler : IRequestHandler<CreateDLCCommand, Result<DLCViewProfile>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateDLCHandler> _logger;

        public CreateDLCHandler(AppDbContext context, ILogger<CreateDLCHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<DLCViewProfile>> Handle(CreateDLCCommand request, CancellationToken cancellationToken)
        {
            // Ensure parent game exists and is not a DLC itself
            var parent = await _context.Games.FirstOrDefaultAsync(g => g.Id == request.GameId, cancellationToken);
            if (parent == null)
                return Result<DLCViewProfile>.Failure("Parent game not found");
            if (parent.IsDLC)
                return Result<DLCViewProfile>.Failure("Parent game cannot be a DLC");

            // Create a new Game record representing the DLC
            var dlcGame = new Steam_TripleBrain.Models.Game
            {
                Id = request.Id == Guid.Empty ? Guid.NewGuid() : request.Id,
                Name = request.Name,
                Price = request.Price,
                Discount = request.Discount,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                Developer = parent.Developer,
                IsDLC = true,
                ParentGameId = parent.Id
            };

            _context.Games.Add(dlcGame);
            await _context.SaveChangesAsync(cancellationToken);

            var profile = DLCMappingProfile.ToProfile(dlcGame);
            return Result<DLCViewProfile>.Success(profile);
        }
    }
}
