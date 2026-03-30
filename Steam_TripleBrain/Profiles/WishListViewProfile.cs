using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.Profiles
{
    public class WishListViewProfile
    {
        public Guid UserId { get; set; }
        public List<Game>? WishGames { get; set; }
    }
}
