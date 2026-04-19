using MediatR;
using System.Collections.Generic;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.Users
{
    public class GetAllUsersCommand : IRequest<Result<List<User>>>
    {
    }
}
