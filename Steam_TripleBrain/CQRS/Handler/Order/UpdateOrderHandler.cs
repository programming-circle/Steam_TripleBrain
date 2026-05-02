using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Order;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;
using OrderItemModel = Steam_TripleBrain.Models.OrderItem;

namespace Steam_TripleBrain.CQRS.Handler.Order
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, Result<Unit>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateOrderHandler> _logger;

        public UpdateOrderHandler(AppDbContext context, ILogger<CreateOrderHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateOrderCommand for Order: {Id}", request.Id);
            var exists = await _context.Orders.AnyAsync(g => g.Id == request.Id, cancellationToken);
            _logger.LogInformation("### UpdateOrderCommand: Checking if order with ID {Id} exists: {Exists}", request.Id, exists);
            if (!exists)
            {
                _logger.LogInformation("No such object in DB");
                return Result<Unit>.Failure("No object to update");
            }

            var order = await _context.Orders
                .Include(o => o.Items)
                //.Include(g => g.Tags)
                //.Include(g => g.DLCs)
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (order == null)
            {
                _logger.LogWarning("Order with ID {Id} not found when attempting to load for update.", request.Id);
                return Result<Unit>.Failure("Order not found.");
            }

            order.Items ??= new List<OrderItemModel>();

            // Merge incoming items into order
            foreach (var incoming in request.Items ?? new List<OrderItemViewProfile>())
            {
                if (incoming == null) continue;

                var incomingId = incoming.Id == Guid.Empty ? Guid.NewGuid() : incoming.Id;
                var existing = order.Items.FirstOrDefault(i => i.Id == incomingId);

                if (existing == null)
                {
                    order.Items.Add(new OrderItemModel
                    {
                        Id = incomingId,
                        OrderId = order.Id,
                        GameId = incoming.GameId,
                        PriceOfItem = incoming.PriceOfItem
                    });
                }
                else
                {
                    existing.GameId = incoming.GameId;
                    existing.PriceOfItem = incoming.PriceOfItem;
                }
            }

            // Recalculate total to avoid drift
            order.TotalPrice = order.Items.Sum(i => i.PriceOfItem);

            _context.Orders.Update(order);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<Unit>.Success(Unit.Value, "Order updated successfully.");
        }
    }
}
