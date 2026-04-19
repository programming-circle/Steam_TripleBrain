using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Query.Tags;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.Tags
{
    public class GetAllTagsQueryHandler : IRequestHandler<GetAllTagsQuery, Result<List<TagViewProfile>>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetAllTagsQueryHandler> _logger;

        public GetAllTagsQueryHandler(AppDbContext context, ILogger<GetAllTagsQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<List<TagViewProfile>>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
        {
            var tags = await _context.Tags.ToListAsync(cancellationToken);
            var profiles = tags.Select(t => MappingProfile.ToProfile(t)).ToList();
            return Result<List<TagViewProfile>>.Success(profiles);
        }
    }
}
