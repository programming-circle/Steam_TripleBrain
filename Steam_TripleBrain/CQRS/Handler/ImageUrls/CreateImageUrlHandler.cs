using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.ImageUrls;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.ImageUrls
{
    public class CreateImageUrlHandler : IRequestHandler<CreateImageUrlCommand, Result<ImageUrl>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateImageUrlHandler> _logger;

        public CreateImageUrlHandler(AppDbContext context, ILogger<CreateImageUrlHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<ImageUrl>> Handle(CreateImageUrlCommand request, CancellationToken cancellationToken)
        {
            var item = ImageMappingProfile.ToImageUrl(request);

            _context.ImageUrls.Add(item);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<ImageUrl>.Success(item);
        }
    }
}
