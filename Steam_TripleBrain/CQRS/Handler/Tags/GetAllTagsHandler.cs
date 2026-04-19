using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Tags;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.Tags
{
    public class GetAllTagsHandler : IRequestHandler<GetAllTagsCommand, Result<List<Tag>>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetAllTagsHandler> _logger;

        public GetAllTagsHandler(AppDbContext context, ILogger<GetAllTagsHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<List<Tag>>> Handle(GetAllTagsCommand request, CancellationToken cancellationToken)
        {
            var tags = await _context.Tags.ToListAsync(cancellationToken);
            return Result<List<Tag>>.Success(tags);
        }
    }
}
