using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Tags;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.Tags
{
    public class GetTagByIdHandler : IRequestHandler<GetTagByIdCommand, Result<Tag>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetTagByIdHandler> _logger;

        public GetTagByIdHandler(AppDbContext context, ILogger<GetTagByIdHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Tag>> Handle(GetTagByIdCommand request, CancellationToken cancellationToken)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
            if (tag == null)
                return Result<Tag>.Failure("Tag not found");

            return Result<Tag>.Success(tag);
        }
    }
}
