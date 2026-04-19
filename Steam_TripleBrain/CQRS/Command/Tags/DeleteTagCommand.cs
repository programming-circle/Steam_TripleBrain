using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.Tags
{
    public class DeleteTagCommand : IRequest<Result<bool>>
    {
        public Guid Id { get; set; }
    }
}
