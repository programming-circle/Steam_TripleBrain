using MediatR;
using Steam_TripleBrain.Profiles;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Query.Reviews
{
    public class GetReviewByIdQuery : IRequest<Result<Steam_TripleBrain.Profiles.Review>>
    {
        public Guid Id { get; set; }
    }
}
