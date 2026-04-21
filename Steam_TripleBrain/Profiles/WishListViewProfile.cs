using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.Profiles
{
    public class WishListViewProfile
    {
        public Guid Id  { get; set; }
        public Guid UserId { get; set; }
        public List<GameViewProfile>? WishGames { get; set; }
    }
}
