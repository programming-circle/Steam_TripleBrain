namespace Steam_TripleBrain.Models
{
    public class TokenLogs
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public string? Username { get; set; } 
        public string? Token { get; set; } 
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow; 
        public DateTime? ExpiredAt { get; set; }
        public bool IsRevoked { get; set; } = false; 
    }
}
