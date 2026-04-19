using MediatR;
using Steam_TripleBrain.Profiles;
using Steam_TripleBrain.Models;
using System.Collections.Generic;

namespace Steam_TripleBrain.CQRS.Query.Orders
{
    public class GetAllOrdersQuery : IRequest<Result<List<OrderViewProfile>>>
    {
    }
}
