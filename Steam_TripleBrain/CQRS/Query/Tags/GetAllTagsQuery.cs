using MediatR;
using System.Collections.Generic;
using Steam_TripleBrain.Profiles;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Query.Tags
{
    public class GetAllTagsQuery : IRequest<Result<List<TagViewProfile>>>
    {
    }
}
