using MediatR;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Command.Game
{
    public class UpdateGameCommand : IRequest<Result<GameViewProfile>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Poster { get; set; }

        public List<string>? Images { get; set; }

        public double Rating { get; set; }

        public string Description { get; set; }
        public List<GenreViewProfile> Genres { get; set; }

        //public List<Tag>? Tags { get; set; }

        public decimal Price { get; set; }

        public int Discount { get; set; }

        public Guid Developer { get; set; }

        //public List<DLC>? DLCs { get; set; }
    }
}
