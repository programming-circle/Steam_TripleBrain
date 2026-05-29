using MediatR;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Command.DLCs
{
    public class UpdateDLCCommand : IRequest<Result<DLCViewProfile>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public string Description { get; set; }
        // optional new parent game id
        public Guid GameId { get; set; }
    }
}
