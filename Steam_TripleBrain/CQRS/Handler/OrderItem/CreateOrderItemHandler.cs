using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.OrderItem;
using Steam_TripleBrain.CQRS.Handler.Game;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;
using Steam_TripleBrain.MappingProfiles;


namespace Steam_TripleBrain.CQRS.Handler.OrderItem
{
    public class CreateOrderItemHandler : IRequestHandler<CreateOrderItemCommand , Result<OrderItemViewProfile>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateGameHandler> _logger;

        public CreateOrderItemHandler(AppDbContext context, ILogger<CreateGameHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<OrderItemViewProfile>> Handle(CreateOrderItemCommand request , CancellationToken cancellationToken)
        {
            _logger.LogInformation("#### CreateOrderItem start work");
            var exists = await _context.OrderItems.AnyAsync(g => g.Id == request.Id, cancellationToken);
            if(exists)
            {
                _logger.LogInformation("#### CreateOrderItem: object with this allready exists");
                return Result<OrderItemViewProfile>.Failure($"OrderItem with {request.Id}, not exists");
            }
            var existsGame = await _context.Games.AnyAsync(g => g.Id == request.GameId, cancellationToken);
            if(!existsGame)
            {
                _logger.LogInformation("#### CreateOrderItem: game with id {request.GameId} not exists", request.GameId);
                return Result<OrderItemViewProfile>.Failure($"Game with {request.GameId} not exists");
            }
            var orderItem = OrderItemMappingProfile.ToOrderItem(request);

            await _context.AddAsync(orderItem);
            await _context.SaveChangesAsync(cancellationToken);

            var orderItemProfile = OrderItemMappingProfile.ToProfile(orderItem);

            _logger.LogInformation("CreateOrderItem
    }
}
: Order item created successfully");
            return Result<OrderItemViewProfile>.Success(orderItemProfile, "Order item created successfully.");
        }