using MediatR;
using Steam_TripleBrain.Profiles;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Query.Genres
{
    public class GetGenreByIdQuery : IRequest<Result<GenreViewProfile>>
    {
        public Guid Id { get; set; }
    }
}
