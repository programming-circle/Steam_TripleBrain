using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.Genres
{
    public class GetGenreByIdCommand : IRequest<Result<Genre>>
    {
        public Guid Id { get; set; }
    }
}
