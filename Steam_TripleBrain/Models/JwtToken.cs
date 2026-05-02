using Steam_TripleBrain.Profiles.Tokens;

namespace Steam_TripleBrain.Models
{
    public class JwtToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public List<TokenJournal>? TokenJournals { get; set; }

    }
}
