using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.DLCs
{
    public class GetDLCByIdCommand : IRequest<Result<DLC>>
    {
        public Guid Id { get; set; }
    }
}
