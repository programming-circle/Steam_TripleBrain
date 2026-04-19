using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.Reviews
{
    public class GetReviewByIdCommand : IRequest<Result<Review>>
    {
        public Guid Id { get; set; }
    }
}
