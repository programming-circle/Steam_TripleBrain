using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Tags;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.Tags
{
    public class DeleteTagHandler : IRequestHandler<DeleteTagCommand, Result<bool>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteTagHandler> _logger;

        public DeleteTagHandler(AppDbContext context, ILogger<DeleteTagHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            var existing = await _context.Tags.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
            if (existing == null)
                return Result<bool>.Failure("Tag not found");

            _context.Tags.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
