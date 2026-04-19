using MediatR;
using System.Collections.Generic;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.Tags
{
    public class GetAllTagsCommand : IRequest<Result<List<Tag>>>
    {
    }
}
