using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.DLCs
{
    public class CreateDLCCommand : IRequest<Result<DLC>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public string Description { get; set; }
        public Guid GameId { get; set; }
    }
}
