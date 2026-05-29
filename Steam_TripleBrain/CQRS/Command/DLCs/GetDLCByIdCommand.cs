using MediatR;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Command.DLCs
{
    public class GetDLCByIdCommand : IRequest<Result<DLCViewProfile>>
    {
        public Guid Id { get; set; }
    }
}
