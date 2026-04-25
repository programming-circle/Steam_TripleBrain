using MediatR;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Query.OrderItem
{
    public class OrderItemDeleteByIdQuery : IRequest<Result<OrderItemViewProfile>>
    {
        public Guid Id { get; set; }
    }
}
