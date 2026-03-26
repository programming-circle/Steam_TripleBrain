namespace Steam_TripleBrain.Models
{
    public class DLC
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public string Description { get; set; }
        public Game Game { get; set; }
    }
}
