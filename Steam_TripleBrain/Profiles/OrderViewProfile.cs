namespace Steam_TripleBrain.Profiles
{
    public class OrderViewProfile
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public decimal TotalPrice { get; set; }

        public List<OrderItemViewProfile> Items { get; set; }
    }
}
