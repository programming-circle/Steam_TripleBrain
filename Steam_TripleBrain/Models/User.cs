using Steam_TripleBrain.Profiles.Tokens;

namespace Steam_TripleBrain.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        
        public string? Email { get; set; }
        public string? Password { get; set; }
        
        public DateTime? BirthDate { get; set; }

        public string? Icon { get; set; }

        public string Role { get; set; } = "User";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }

        public List<Game>? PurchasedGames { get; set; }

        public List<TokenJournal>? TokenJournals { get; set; }
        // public List<DLC>? DLCs { get; set; }

        //Planing of adding roles for users, but not sure if it will be needed in the future
        //public Role UserRole { get; set;}

    }
}
