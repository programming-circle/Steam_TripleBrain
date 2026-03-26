namespace Steam_TripleBrain.Models
{
    public class GenreToGame
    {
        // This class is used to create a many-to-many relationship between genres and games
        public Guid GenreId { get; set; }
        public Guid GameId { get; set; }
    }
}
