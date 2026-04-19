using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Query.Orders;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.Orders
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, Result<List<OrderViewProfile>>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetAllOrdersQueryHandler> _logger;

        public GetAllOrdersQueryHandler(AppDbContext context, ILogger<GetAllOrdersQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<List<OrderViewProfile>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders.Include(o => o.Items).ToListAsync(cancellationToken);
            var profiles = orders.Select(o => MappingProfile.ToProfile(o)).ToList();
            return Result<List<OrderViewProfile>>.Success(profiles);
        }
    }
}
