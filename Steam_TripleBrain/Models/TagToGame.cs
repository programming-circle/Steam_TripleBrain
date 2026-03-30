namespace Steam_TripleBrain.Models
{
    public class TagToGame
    {
        // This class is used to create a many-to-many relationship between tags and games
        public Guid TagId { get; set; }
        public Guid GameId { get; set; }
    }
}
