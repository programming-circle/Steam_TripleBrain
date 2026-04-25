using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Query.OrderItem;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.OrderItem
{
    public class GetAllOrderItemsHandler : IRequestHandler<OrderItemGetAllQuery, Result<List<OrderItemViewProfile>>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetAllOrderItemsHandler> _logger;

        public GetAllOrderItemsHandler(AppDbContext context, ILogger<GetAllOrderItemsHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Result<List<OrderItemViewProfile>>> Handle(OrderItemGetAllQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetAllOrderItems start work");

            var items = await _context.OrderItems
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var result = items
                .Select(OrderItemMappingProfile.ToProfile)
                .ToList();

            return Result<List<OrderItemViewProfile>>
                .Success(result, "OrderItems fetched successfully");
        }
    }
}
