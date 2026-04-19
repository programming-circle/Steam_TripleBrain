using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.OrderItems
{
    public class DeleteOrderItemCommand : IRequest<Result<bool>>
    {
        public Guid Id { get; set; }
    }
}
