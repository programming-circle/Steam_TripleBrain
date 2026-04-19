using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.Reviews
{
    public class DeleteReviewCommand : IRequest<Result<bool>>
    {
        public Guid Id { get; set; }
    }
}
