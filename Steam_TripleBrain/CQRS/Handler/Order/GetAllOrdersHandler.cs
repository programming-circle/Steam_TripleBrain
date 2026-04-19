using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Orders;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Handler.Orders
{
    public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersCommand, Result<List<Order>>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetAllOrdersHandler> _logger;

        public GetAllOrdersHandler(AppDbContext context, ILogger<GetAllOrdersHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<List<Order>>> Handle(GetAllOrdersCommand request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders
                .Include(o => o.Items)
                .ToListAsync(cancellationToken);

            return Result<List<Order>>.Success(orders);
        }
    }
}
