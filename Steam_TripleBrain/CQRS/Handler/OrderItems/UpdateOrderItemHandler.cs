using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.OrderItems;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Handler.OrderItems
{
    public class UpdateOrderItemHandler : IRequestHandler<UpdateOrderItemCommand, Result<OrderItem>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UpdateOrderItemHandler> _logger;

        public UpdateOrderItemHandler(AppDbContext context, ILogger<UpdateOrderItemHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<OrderItem>> Handle(UpdateOrderItemCommand request, CancellationToken cancellationToken)
        {
            var existing = await _context.OrderItems.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
            if (existing == null)
                return Result<OrderItem>.Failure("OrderItem not found");

            // Map updated values using mapping profile (preserve Id)
            var updated = MappingProfiles.OrderItemMappingProfile.ToOrderItem(new CreateOrderItemCommand
            {
                Id = request.Id,
                OrderId = request.OrderId,
                GameId = request.GameId,
                DLCId = request.DLCId,
                PriceOfItem = request.PriceOfItem
            });

            _context.Entry(existing).CurrentValues.SetValues(updated);
            await _context.SaveChangesAsync(cancellationToken);

            // Recalculate order total
            var order = await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == existing.OrderId, cancellationToken);
            if (order != null)
            {
                order.TotalPrice = order.Items.Sum(i => i.PriceOfItem);
                _context.Orders.Update(order);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Result<OrderItem>.Success(existing);
        }
    }
}
