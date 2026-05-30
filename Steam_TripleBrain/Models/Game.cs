namespace Steam_TripleBrain.Models
{
    public class Game
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Poster { get; set; }

        public List<string>? Images { get; set; }

        public double Rating { get; set; } // Possibly i gonna add system relating on ratings from Reviews.

        public string? Description { get; set; }
        public List<string>? Genres { get; set; }

        //public List<Tag>? Tags { get; set; }

        public decimal Price { get; set; }

        public int Discount { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid Developer { get; set; }

        // Indicates that this Game record is a DLC rather than a base game
        public bool IsDLC { get; set; }

        // If IsDLC == true, this points to the parent/base game
        public Guid? ParentGameId { get; set; }

        //public List<DLC>? DLCs { get; set; }
        //public object GameMappintProfile { get; internal set; }
    }
}
