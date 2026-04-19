using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Orders;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.Orders
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Result<Order>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateOrderHandler> _logger;

        public CreateOrderHandler(AppDbContext context, ILogger<CreateOrderHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Order>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return Result<Order>.Failure("Request is null");

            var order = MappingProfile.ToOrder(request);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<Order>.Success(order);
        }
    }
}
