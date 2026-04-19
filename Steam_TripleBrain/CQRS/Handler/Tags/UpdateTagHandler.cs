using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Tags;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.Tags
{
    public class UpdateTagHandler : IRequestHandler<UpdateTagCommand, Result<Tag>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateTagHandler> _logger;

        public UpdateTagHandler(AppDbContext context, ILogger<UpdateTagHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Tag>> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var existing = await _context.Tags.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
            if (existing == null)
                return Result<Tag>.Failure("Tag not found");

            existing.Name = request.Name;

            _context.Tags.Update(existing);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<Tag>.Success(existing);
        }
    }
}
