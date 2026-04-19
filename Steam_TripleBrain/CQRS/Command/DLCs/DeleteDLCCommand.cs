using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.DLCs
{
    public class DeleteDLCCommand : IRequest<Result<bool>>
    {
        public Guid Id { get; set; }
    }
}
