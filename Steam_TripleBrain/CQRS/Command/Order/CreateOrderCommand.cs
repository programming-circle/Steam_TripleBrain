using MediatR;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Command.Orders
{
    public class CreateOrderCommand : IRequest<Result<Order>>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}
