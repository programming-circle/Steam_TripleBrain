using MediatR;
using Steam_TripleBrain.Profiles;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Query.DLCs
{
    public class GetDLCByIdQuery : IRequest<Result<DLCViewProfile>>
    {
        public Guid Id { get; set; }
    }
}
