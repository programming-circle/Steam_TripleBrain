using MediatR;
using Steam_TripleBrain.Profiles;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Query.Tags
{
    public class GetTagByIdQuery : IRequest<Result<TagViewProfile>>
    {
        public Guid Id { get; set; }
    }
}
