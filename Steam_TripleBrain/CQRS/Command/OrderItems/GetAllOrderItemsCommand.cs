using MediatR;
using System.Collections.Generic;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.OrderItems
{
    public class GetAllOrderItemsCommand : IRequest<Result<List<OrderItem>>>
    {
    }
}
