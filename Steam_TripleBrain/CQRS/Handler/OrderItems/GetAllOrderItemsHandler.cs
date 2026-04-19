using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.OrderItems;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Handler.OrderItems
{
    public class GetAllOrderItemsHandler : IRequestHandler<GetAllOrderItemsCommand, Result<List<OrderItem>>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetAllOrderItemsHandler> _logger;

        public GetAllOrderItemsHandler(AppDbContext context, ILogger<GetAllOrderItemsHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<List<OrderItem>>> Handle(GetAllOrderItemsCommand request, CancellationToken cancellationToken)
        {
            var items = await _context.OrderItems.ToListAsync(cancellationToken);
            return Result<List<OrderItem>>.Success(items);
        }
    }
}
