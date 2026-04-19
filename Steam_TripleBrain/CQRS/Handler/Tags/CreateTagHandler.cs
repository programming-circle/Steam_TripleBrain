using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Tags;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.Tags
{
    public class CreateTagHandler : IRequestHandler<CreateTagCommand, Result<Tag>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateTagHandler> _logger;

        public CreateTagHandler(AppDbContext context, ILogger<CreateTagHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Tag>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = new Tag
            {
                Id = request.Id == Guid.Empty ? Guid.NewGuid() : request.Id,
                Name = request.Name
            };

            _context.Tags.Add(tag);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<Tag>.Success(tag);
        }
    }
}
