using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.Profiles
{
    public class UserViewProfile
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public string? Icon { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<Game> PurchasedGames { get; set; }
        // public List<DLC> DLCs { get; set; }
    }
}
