using MediatR;
using Steam_TripleBrain.Profiles;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Query.ImageUrls
{
    public class GetImageUrlByIdQuery : IRequest<Result<ImageUrlViewProfile>>
    {
        public Guid Id { get; set; }
    }
}
