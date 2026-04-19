using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.Orders
{
    public class GetOrderByIdCommand : IRequest<Result<Order>>
    {
        public Guid Id { get; set; }
    }
}
