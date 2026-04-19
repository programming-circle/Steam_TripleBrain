using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Orders;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.Orders
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, Result<Order>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateOrderHandler> _logger;

        public UpdateOrderHandler(AppDbContext context, ILogger<UpdateOrderHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Order>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var existing = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (existing == null)
                return Result<Order>.Failure("Order not found");

            // Build updated order using mapper and preserve CreatedAt
            var updatedOrder = MappingProfile.ToOrder(request, existing.Id);
            updatedOrder.CreatedAt = existing.CreatedAt;

            // remove old items
            _context.OrderItems.RemoveRange(existing.Items);

            // add new items
            _context.Orders.Update(updatedOrder);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<Order>.Success(updatedOrder);
        }
    }
}
