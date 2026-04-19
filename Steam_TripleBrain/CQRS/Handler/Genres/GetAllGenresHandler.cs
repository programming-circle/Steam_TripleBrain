using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Genres;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.Genres
{
    public class GetAllGenresHandler : IRequestHandler<GetAllGenresCommand, Result<List<Genre>>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetAllGenresHandler> _logger;

        public GetAllGenresHandler(AppDbContext context, ILogger<GetAllGenresHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<List<Genre>>> Handle(GetAllGenresCommand request, CancellationToken cancellationToken)
        {
            var genres = await _context.Genres.ToListAsync(cancellationToken);
            return Result<List<Genre>>.Success(genres);
        }
    }
}
