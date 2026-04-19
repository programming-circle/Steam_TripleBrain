using MediatR;
using System.Collections.Generic;
using Steam_TripleBrain.Profiles;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Query.DLCs
{
    public class GetAllDLCsQuery : IRequest<Result<List<DLCViewProfile>>>
    {
    }
}
