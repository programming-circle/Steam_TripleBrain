using MediatR;
using Steam_TripleBrain.CQRS.Handler.OrderItem;
using Steam_TripleBrain.CQRS.Query.Order;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.Order
{
    public class DeleteOrderByIdHandler : IRequestHandler<OrderDeleteByIdQueryRequest, Result<OrderViewProfile>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteOrderItemByIdHandler> _logger;

        public DeleteOrderByIdHandler(AppDbContext context, ILogger<DeleteOrderItemByIdHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<OrderViewProfile>> Handle(OrderDeleteByIdQueryRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start working of command DeleteOrderById");
            Models.Order order = await _context.Orders.FindAsync(request.Id);
            if (order == null)
            {
                _logger.LogInformation("No order with {order.Id} exists", request.Id);
                return Result<OrderViewProfile>.Failure($"order with id: {request.Id} not exist");
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync(cancellationToken);
            var orderViewProfile = OrderMappingProfile.ToProfile(order);
            return Result<OrderViewProfile>.Success(orderViewProfile, "Order deleted successfully.");
        }
    }
}