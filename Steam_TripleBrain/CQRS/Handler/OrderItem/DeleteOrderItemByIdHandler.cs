using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Query.OrderItem;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.OrderItem
{
    public class DeleteOrderItemByIdHandler : IRequestHandler<OrderItemDeleteByIdQuery, Result<OrderItemViewProfile>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteOrderItemByIdHandler> _logger;

        public DeleteOrderItemByIdHandler(AppDbContext context, ILogger<DeleteOrderItemByIdHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<OrderItemViewProfile>> Handle(OrderItemDeleteByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("DeleteOrderItem start work");

            var item = await _context.OrderItems
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (item == null)
            {
                _logger.LogWarning("OrderItem not found: {Id}", request.Id);
                return Result<OrderItemViewProfile>
                    .Failure($"OrderItem with id {request.Id} not found");
            }

            _context.OrderItems.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);

            var profile = OrderItemMappingProfile.ToProfile(item);

            return Result<OrderItemViewProfile>
                .Success(profile, "OrderItem deleted successfully");
        }
    }
}
