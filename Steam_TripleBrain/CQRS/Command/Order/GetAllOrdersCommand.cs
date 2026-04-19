using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.Orders
{
    public class GetAllOrdersCommand : IRequest<Result<List<Order>>>
    {
    }
}
