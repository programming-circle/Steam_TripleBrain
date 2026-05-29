using MediatR;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Query.Genre
{
    public class GetAllGenreQuery : IRequest<Result<List<GenreViewProfile>>>
    {

    }
}
