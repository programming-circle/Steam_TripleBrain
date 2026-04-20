using MediatR;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Query.Game
{
    public class GetAllGamesQueryRequest : IRequest<Result<List<GameViewProfile>>>
    {
        // Pagination
        public int Page { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 1 ? 10 : (value > 50 ? 50 : value);
        }

        // Filtering
        public string? Name { get; set; }
        public double RatingFrom { get; set; }
        public double RatingTo { get; set; }
        public decimal PriceFrom { get; set; }
        public decimal PriceTo { get; set; }

        // Sorting
        public string SortBy { get; set; } = "name";
        public string SortDir { get; set; } = "asc";
    }
}
