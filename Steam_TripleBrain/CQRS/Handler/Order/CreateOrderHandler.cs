using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Order;
using Steam_TripleBrain.CQRS.Command.OrderItem;
using Steam_TripleBrain.CQRS.Handler.Game;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.Order
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Result<OrderViewProfile>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateOrderHandler> _logger;

        public CreateOrderHandler(AppDbContext context, ILogger<CreateOrderHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<OrderViewProfile>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("#### CreateOrder start work");
            var exists = await _context.Orders.AnyAsync(g => g.Id == request.Id, cancellationToken);
            if (exists)
            {
                _logger.LogInformation("#### CreateOrder: object with this allready exists");
                return Result<OrderViewProfile>.Failure($"Order with {request.Id}, not exists");
            }

            var order = OrderMappingProfile.ToOrder(request);

            await _context.AddAsync(order);
            await _context.SaveChangesAsync(cancellationToken);

            var orderProfile = OrderMappingProfile.ToProfile(order);

            return Result<OrderViewProfile>.Success(orderProfile, "Order item created successfully.");
        }
    }

}
