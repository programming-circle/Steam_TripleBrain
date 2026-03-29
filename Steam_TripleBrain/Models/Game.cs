namespace Steam_TripleBrain.Models
{
    public class Game
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
        //public object GameMappintProfile { get; internal set; }
    }
}
