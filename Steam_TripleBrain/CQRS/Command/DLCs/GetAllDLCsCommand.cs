using MediatR;
using System.Collections.Generic;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Command.DLCs
{
    public class GetAllDLCsCommand : IRequest<Result<List<DLC>>>
    {
    }
}
