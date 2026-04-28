using MediatR;
using Steam_TripleBrain.CQRS.Handler.Game;
using Steam_TripleBrain.CQRS.Query.Order;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.CQRS.Handler.Order
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQueryRequest, Result<OrderViewProfile>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetOrderByIdQueryHandler> _logger;

        public GetOrderByIdQueryHandler(AppDbContext context, ILogger<GetOrderByIdQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<OrderViewProfile>> Handle(GetOrderByIdQueryRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start working of command GetOrderById");
            Models.Order order = await _context.Orders.FindAsync(request.Id);
            if (order == null)
            {
                _logger.LogInformation("No order with {order.Id} exists", request.Id);
                return Result<OrderViewProfile>.Failure($"order with id: {request.Id} not exist");
            }
            var orderViewProfile = OrderMappingProfile.ToProfile(order);
            return Result<OrderViewProfile>.Success(orderViewProfile, "Order created successfully.");
        }
    }
}
