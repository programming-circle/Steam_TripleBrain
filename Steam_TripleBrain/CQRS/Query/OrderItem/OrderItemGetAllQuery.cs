using MediatR;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Query.OrderItem
{
    public class OrderItemGetAllQuery : IRequest<Result<List<OrderItemViewProfile>>>
    {
    }
}
