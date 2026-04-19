using MediatR;
using System.Collections.Generic;
using Steam_TripleBrain.Profiles;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Query.ImageUrls
{
    public class GetAllImageUrlsQuery : IRequest<Result<List<ImageUrlViewProfile>>>
    {
    }
}
