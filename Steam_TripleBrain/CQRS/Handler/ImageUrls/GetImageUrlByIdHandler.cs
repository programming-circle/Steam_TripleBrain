using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Query.ImageUrls;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.ImageUrls
{
    public class GetImageUrlByIdHandler : IRequestHandler<GetImageUrlByIdQuery, Result<ImageUrlViewProfile>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetImageUrlByIdHandler> _logger;

        public GetImageUrlByIdHandler(AppDbContext context, ILogger<GetImageUrlByIdHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<ImageUrlViewProfile>> Handle(GetImageUrlByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _context.ImageUrls.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
            if (item == null)
                return Result<ImageUrlViewProfile>.Failure("Image not found");

            var profile = ImageMappingProfile.ToProfile(item);
            return Result<ImageUrlViewProfile>.Success(profile);
        }
    }
}
