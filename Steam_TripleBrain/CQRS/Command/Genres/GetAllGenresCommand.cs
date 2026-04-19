using MediatR;
using System.Collections.Generic;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.Genres
{
    public class GetAllGenresCommand : IRequest<Result<List<Genre>>>
    {
    }
}
