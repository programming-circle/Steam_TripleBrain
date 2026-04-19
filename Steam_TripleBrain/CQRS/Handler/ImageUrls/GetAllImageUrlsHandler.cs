using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Query.ImageUrls;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.ImageUrls
{
    public class GetAllImageUrlsHandler : IRequestHandler<GetAllImageUrlsQuery, Result<List<ImageUrlViewProfile>>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetAllImageUrlsHandler> _logger;

        public GetAllImageUrlsHandler(AppDbContext context, ILogger<GetAllImageUrlsHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<List<ImageUrlViewProfile>>> Handle(GetAllImageUrlsQuery request, CancellationToken cancellationToken)
        {
            var items = await _context.ImageUrls.ToListAsync(cancellationToken);
            var profiles = items.Select(i => MappingProfile.ToProfile(i)).ToList();
            return Result<List<ImageUrlViewProfile>>.Success(profiles);
        }
    }
}
