using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.Users
{
    public class GetUserByIdCommand : IRequest<Result<User>>
    {
        public Guid Id { get; set; }
    }
}
