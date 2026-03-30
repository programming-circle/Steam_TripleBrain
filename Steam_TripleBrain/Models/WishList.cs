using Microsoft.EntityFrameworkCore;

namespace Steam_TripleBrain.Models
{
    [Keyless]
    public class WishList
    {
        public Guid UserId { get; set; }
        public List<Game>? WishGames { get; set; }
    }
}
