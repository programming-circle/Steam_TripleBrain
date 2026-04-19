using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.Genres
{
    public class DeleteGenreCommand : IRequest<Result<bool>>
    {
        public Guid Id { get; set; }
    }
}
