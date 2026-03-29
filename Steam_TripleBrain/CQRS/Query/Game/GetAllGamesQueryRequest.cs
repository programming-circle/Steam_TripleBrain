using MediatR;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Query.Game
{
    public class GetAllGamesQueryRequest : IRequest<Result<List<GameViewProfile>>>
    {
    }
}
