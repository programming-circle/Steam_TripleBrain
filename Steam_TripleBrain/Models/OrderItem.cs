namespace Steam_TripleBrain.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
        
        public Guid? GameId { get; set; }

        public Guid? DLCId { get; set; }

        public decimal PriceOfItem { get; set; }
    }
}
