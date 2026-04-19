using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.ImageUrls
{
    public class UpdateImageUrlCommand : IRequest<Result<ImageUrl>>
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
    }
}
