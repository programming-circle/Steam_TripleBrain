using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Orders;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Handler.Orders
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, Result<bool>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteOrderHandler> _logger;

        public DeleteOrderHandler(AppDbContext context, ILogger<DeleteOrderHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var existing = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (existing == null)
                return Result<bool>.Failure("Order not found");

            _context.OrderItems.RemoveRange(existing.Items);
            _context.Orders.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
