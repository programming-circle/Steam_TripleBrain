using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.OrderItems;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Handler.OrderItems
{
    public class DeleteOrderItemHandler : IRequestHandler<DeleteOrderItemCommand, Result<bool>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteOrderItemHandler> _logger;

        public DeleteOrderItemHandler(AppDbContext context, ILogger<DeleteOrderItemHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(DeleteOrderItemCommand request, CancellationToken cancellationToken)
        {
            var existing = await _context.OrderItems.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
            if (existing == null)
                return Result<bool>.Failure("OrderItem not found");

            var orderId = existing.OrderId;
            _context.OrderItems.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);

            // Recalculate order total
            var order = await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
            if (order != null)
            {
                order.TotalPrice = order.Items.Sum(i => i.PriceOfItem);
                _context.Orders.Update(order);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Result<bool>.Success(true);
        }
    }
}
