using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.ImageUrls
{
    public class GetImageUrlByIdCommand : IRequest<Result<ImageUrl>>
    {
        public Guid Id { get; set; }
    }
}
