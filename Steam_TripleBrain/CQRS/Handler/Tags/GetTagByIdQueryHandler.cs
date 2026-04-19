using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Query.Tags;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.Tags
{
    public class GetTagByIdQueryHandler : IRequestHandler<GetTagByIdQuery, Result<TagViewProfile>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetTagByIdQueryHandler> _logger;

        public GetTagByIdQueryHandler(AppDbContext context, ILogger<GetTagByIdQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<TagViewProfile>> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
            if (tag == null)
                return Result<TagViewProfile>.Failure("Tag not found");

            var profile = MappingProfile.ToProfile(tag);
            return Result<TagViewProfile>.Success(profile);
        }
    }
}
