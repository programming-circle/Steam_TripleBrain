namespace Steam_TripleBrain.Profiles
{
    public class DLCViewProfile
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public string Description { get; set; }
        public Guid GameId { get; set; }
    }
}
