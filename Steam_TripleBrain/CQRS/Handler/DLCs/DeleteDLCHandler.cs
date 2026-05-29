using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.DLCs;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.DLCs
{
    public class DeleteDLCHandler : IRequestHandler<DeleteDLCCommand, Result<bool>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteDLCHandler> _logger;

        public DeleteDLCHandler(AppDbContext context, ILogger<DeleteDLCHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(DeleteDLCCommand request, CancellationToken cancellationToken)
        {
            var existing = await _context.Games.FirstOrDefaultAsync(g => g.Id == request.Id && g.IsDLC, cancellationToken);
            if (existing == null)
                return Result<bool>.Failure("DLC not found");

            _context.Games.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
