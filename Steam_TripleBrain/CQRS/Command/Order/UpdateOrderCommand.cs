using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.Orders
{
    public class UpdateOrderCommand : IRequest<Result<Order>>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}
