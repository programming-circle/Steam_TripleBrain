using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.Orders
{
    public class DeleteOrderCommand : IRequest<Result<bool>>
    {
        public Guid Id { get; set; }
    }
}
