using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.OrderItems
{
    public class UpdateOrderItemCommand : IRequest<Result<OrderItem>>
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid? GameId { get; set; }
        public Guid? DLCId { get; set; }
        public decimal PriceOfItem { get; set; }
    }
}
