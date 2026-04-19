using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.Genres
{
    public class UpdateGenreCommand : IRequest<Result<Genre>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
