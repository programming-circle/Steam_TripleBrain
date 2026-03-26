namespace Steam_TripleBrain.Models
{
    public class FriendShip
    {
        public Guid UserId { get; set; }
        public Guid FriendId { get; set; }
        public string Status { get; set; } = "neutral";
    }
}
