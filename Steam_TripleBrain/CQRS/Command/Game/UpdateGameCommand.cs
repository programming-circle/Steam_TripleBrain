using MediatR;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Command.Game
{
    public class UpdateGameCommand : IRequest<Result<GameViewProfile>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ImageUrl Poster { get; set; }

        public List<ImageUrl>? Images { get; set; }

        public double Rating { get; set; }

        public string Description { get; set; }
        public List<Genre> Genres { get; set; }

        public List<Tag>? Tags { get; set; }

        public decimal Price { get; set; }

        public int Discount { get; set; }

        public Guid Author { get; set; }

        public List<DLC>? DLCs { get; set; }
    }
}
