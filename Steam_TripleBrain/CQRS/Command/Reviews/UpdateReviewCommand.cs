using MediatR;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.Reviews
{
    public class UpdateReviewCommand : IRequest<Result<Review>>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid GameId { get; set; }
        public double Rating { get; set; }
        public string? Text { get; set; }
    }
}
