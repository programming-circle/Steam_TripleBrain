using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.Profiles
{
    public class GameViewProfile
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ImageUrlViewProfile Poster { get; set; }

        public List<ImageUrlViewProfile>? Images { get; set; }

        public double Rating { get; set; }

        public string Description { get; set; }
        public List<GenreViewProfile> Genres { get; set; }

        public List<TagViewProfile>? Tags { get; set; }

        public decimal Price { get; set; }

        public int Discount { get; set; }

        public Guid Author { get; set; }

        public List<DLCViewProfile>? DLCs { get; set; }
    }
}
