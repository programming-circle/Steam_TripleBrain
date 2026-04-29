using MediatR;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Command.Genre
{
    public class CreateGenreCommand : IRequest<Result<GenreViewProfile>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
