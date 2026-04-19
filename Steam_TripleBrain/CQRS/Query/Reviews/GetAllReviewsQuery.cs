using MediatR;
using System.Collections.Generic;
using Steam_TripleBrain.Profiles;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Query.Reviews
{
    public class GetAllReviewsQuery : IRequest<Result<List<Steam_TripleBrain.Profiles.Review>>>
    {
    }
}
