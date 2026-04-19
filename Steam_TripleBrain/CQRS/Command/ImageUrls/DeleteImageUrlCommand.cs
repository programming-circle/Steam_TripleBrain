using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.ImageUrls
{
    public class DeleteImageUrlCommand : IRequest<Result<bool>>
    {
        public Guid Id { get; set; }
    }
}
