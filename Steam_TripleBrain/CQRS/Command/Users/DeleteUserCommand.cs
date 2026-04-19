using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.Users
{
    public class DeleteUserCommand : IRequest<Result<bool>>
    {
        public Guid Id { get; set; }
    }
}
