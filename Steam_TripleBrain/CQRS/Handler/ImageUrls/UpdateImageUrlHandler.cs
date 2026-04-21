using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.ImageUrls;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.ImageUrls
{
    public class UpdateImageUrlHandler : IRequestHandler<UpdateImageUrlCommand, Result<ImageUrl>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateImageUrlHandler> _logger;

        public UpdateImageUrlHandler(AppDbContext context, ILogger<UpdateImageUrlHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<ImageUrl>> Handle(UpdateImageUrlCommand request, CancellationToken cancellationToken)
        {
            var existing = await _context.ImageUrls.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
            if (existing == null)
                return Result<ImageUrl>.Failure("Image not found");

            var updated = new ImageUrl
            {
                Url = request.Url
            };
            updated.Id = existing.Id;

            _context.Entry(existing).CurrentValues.SetValues(updated);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<ImageUrl>.Success(existing);
        }
    }
}
