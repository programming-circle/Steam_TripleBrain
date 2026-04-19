using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Orders;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Handler.Orders
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdCommand, Result<Order>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetOrderByIdHandler> _logger;

        public GetOrderByIdHandler(AppDbContext context, ILogger<GetOrderByIdHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Order>> Handle(GetOrderByIdCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

            if (order == null)
                return Result<Order>.Failure("Order not found");

            return Result<Order>.Success(order);
        }
    }
}
