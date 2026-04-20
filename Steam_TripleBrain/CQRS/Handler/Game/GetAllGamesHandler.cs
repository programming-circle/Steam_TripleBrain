using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Query.Game;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;
// no pagination namespace needed here

namespace Steam_TripleBrain.CQRS.Handler.Game
{
    public class GetAllGamesHandler : IRequestHandler<GetAllGamesQueryRequest, Result<List<GameViewProfile>>>
    {

        private readonly AppDbContext _context;
        private readonly ILogger<GetAllGamesHandler> _logger;

        public GetAllGamesHandler(AppDbContext context, ILogger<GetAllGamesHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<List<GameViewProfile>>> Handle(GetAllGamesQueryRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllGamesQueryRequest");
            IQueryable<Models.Game> query = _context.Games.AsNoTracking()
                .Include(g => g.Poster)
                .Include(g => g.Images)
                .Include(g => g.Genres)
                .Include(g => g.Tags)
                .Include(g => g.DLCs);

            // IIf something gonna be filtered.
            bool hasAnyFilter = !string.IsNullOrWhiteSpace(request.Name)
                                || request.RatingFrom > 0 || request.RatingTo > 0
                                || request.PriceFrom > 0 || request.PriceTo > 0
                                || !string.IsNullOrWhiteSpace(request.SortBy) || !string.IsNullOrWhiteSpace(request.SortDir);

            // Basic behavior: if no params provided, return default
            if (!hasAnyFilter && request.Page <= 1 && request.PageSize == 10)
            {
                var page = 1;
                var pageSize = 10;

                var items = await query.OrderBy(g => g.Name)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(g => new GameViewProfile
                    {
                        Id = g.Id,
                        Name = g.Name,
                        Rating = g.Rating,
                        Price = g.Price,
                        Discount = g.Discount,
                        Poster = g.Poster == null ? null : new ImageUrlViewProfile { Id = g.Poster.Id, Url = g.Poster.Url }
                    })
                    .ToListAsync(cancellationToken);

                return Result<List<GameViewProfile>>.Success(items, "Ok");
            }

            // Apply filters
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                query = query.Where(p => p.Name.Contains(request.Name));
            }
            if (request.RatingFrom > 0)
            {
                query = query.Where(p => p.Rating >= request.RatingFrom);
            }
            if (request.RatingTo > 0)
            {
                query = query.Where(p => p.Rating <= request.RatingTo);
            }
            if (request.PriceFrom > 0)
            {
                query = query.Where(p => p.Price >= request.PriceFrom);
            }
            if (request.PriceTo > 0)
            {
                query = query.Where(p => p.Price <= request.PriceTo);
            }

            // Sorting
            bool desc = string.Equals(request.SortDir, "desc", StringComparison.OrdinalIgnoreCase);
            var sortBy = (request.SortBy ?? "name").ToLower();
            query = sortBy switch
            {
                "price" => desc ? query.OrderByDescending(o => o.Price) : query.OrderBy(o => o.Price),
                "rating" => desc ? query.OrderByDescending(o => o.Rating) : query.OrderBy(o => o.Rating),
                _ => desc ? query.OrderByDescending(o => o.Name) : query.OrderBy(o => o.Name),
            };

            var pageNum = request.Page < 1 ? 1 : request.Page;
            var pageSizeReq = request.PageSize < 1 ? 10 : request.PageSize;

            var itemsFull = await query.Skip((pageNum - 1) * pageSizeReq)
                .Take(pageSizeReq)
                .ToListAsync(cancellationToken);

            var profiles = itemsFull.Select(GameMappingProfile.ToProfile).ToList();
            return Result<List<GameViewProfile>>.Success(profiles, "Ok");
        }
    }
}
