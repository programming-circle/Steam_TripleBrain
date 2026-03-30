using Microsoft.EntityFrameworkCore;

namespace Steam_TripleBrain.Models
{
    
    public class WishList
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<Game>? WishGames { get; set; }
    }
}
