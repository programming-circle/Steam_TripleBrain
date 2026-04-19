using MediatR;
using Steam_TripleBrain.Profiles;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Query.Orders
{
    public class GetOrderByIdQuery : IRequest<Result<OrderViewProfile>>
    {
        public Guid Id { get; set; }
    }
}
