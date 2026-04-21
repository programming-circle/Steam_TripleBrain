using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.OrderItems;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Handler.OrderItems
{
    public class CreateOrderItemHandler : IRequestHandler<CreateOrderItemCommand, Result<OrderItem>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateOrderItemHandler> _logger;

        public CreateOrderItemHandler(AppDbContext context, ILogger<CreateOrderItemHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<OrderItem>> Handle(CreateOrderItemCommand request, CancellationToken cancellationToken)
        {
            // Map request to domain model using mapping profile
            var item = OrderItemMappingProfile.ToOrderItem(request);

            _context.OrderItems.Add(item);
            await _context.SaveChangesAsync(cancellationToken);

            // Recalculate order total
            var order = await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == item.OrderId, cancellationToken);
            if (order != null)
            {
                order.TotalPrice = order.Items.Sum(i => i.PriceOfItem);
                _context.Orders.Update(order);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Result<OrderItem>.Success(item);
        }
    }
}
