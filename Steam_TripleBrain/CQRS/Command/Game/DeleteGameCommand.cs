using MediatR;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Command.Game
{
    public class DeleteGameCommand : IRequest<Result<GameViewProfile>>
    {
        public Guid Id { get; set; }
    }
}
