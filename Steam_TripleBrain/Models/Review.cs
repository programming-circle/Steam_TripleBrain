namespace Steam_TripleBrain.Models
{
    public class Review
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid GameId { get; set; }
        public double Rating { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
