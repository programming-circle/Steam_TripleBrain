namespace Steam_TripleBrain.Models
{
    public class WishList
    {
        public Guid UserId { get; set; }
        public List<Game> WishGames { get; set; }
    }
}
