using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.OrderItems;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.CQRS.Handler.OrderItems
{
    public class GetOrderItemByIdHandler : IRequestHandler<GetOrderItemByIdCommand, Result<OrderItem>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetOrderItemByIdHandler> _logger;

        public GetOrderItemByIdHandler(AppDbContext context, ILogger<GetOrderItemByIdHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<OrderItem>> Handle(GetOrderItemByIdCommand request, CancellationToken cancellationToken)
        {
            var item = await _context.OrderItems.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
            if (item == null)
                return Result<OrderItem>.Failure("OrderItem not found");

            return Result<OrderItem>.Success(item);
        }
    }
}
