using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.ImageUrls;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.ImageUrls
{
    public class DeleteImageUrlHandler : IRequestHandler<DeleteImageUrlCommand, Result<bool>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteImageUrlHandler> _logger;

        public DeleteImageUrlHandler(AppDbContext context, ILogger<DeleteImageUrlHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(DeleteImageUrlCommand request, CancellationToken cancellationToken)
        {
            var existing = await _context.ImageUrls.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
            if (existing == null)
                return Result<bool>.Failure("Image not found");

            _context.ImageUrls.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
