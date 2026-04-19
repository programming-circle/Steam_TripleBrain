using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.Tags
{
    public class GetTagByIdCommand : IRequest<Result<Tag>>
    {
        public Guid Id { get; set; }
    }
}
