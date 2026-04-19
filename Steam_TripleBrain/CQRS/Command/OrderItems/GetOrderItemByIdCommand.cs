using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.OrderItems
{
    public class GetOrderItemByIdCommand : IRequest<Result<OrderItem>>
    {
        public Guid Id { get; set; }
    }
}
