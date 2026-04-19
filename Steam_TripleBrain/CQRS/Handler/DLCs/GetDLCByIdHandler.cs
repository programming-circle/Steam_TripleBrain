using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.DLCs;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.DLCs
{
    public class GetDLCByIdHandler : IRequestHandler<GetDLCByIdCommand, Result<DLC>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetDLCByIdHandler> _logger;

        public GetDLCByIdHandler(AppDbContext context, ILogger<GetDLCByIdHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<DLC>> Handle(GetDLCByIdCommand request, CancellationToken cancellationToken)
        {
            var dlc = await _context.DLCs.Include(d => d.Game).FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);
            if (dlc == null)
                return Result<DLC>.Failure("DLC not found");

            return Result<DLC>.Success(dlc);
        }
    }
}
