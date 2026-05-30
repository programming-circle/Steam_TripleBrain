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
        private readonly ILogger<CreateOrderItemHandler> _logger;

        public CreateOrderItemHandler(AppDbContext context, ILogger<CreateOrderItemHandler> logger)
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

            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            if (order != null)
            {
                var user = await _context.Users
                    .Include(u => u.PurchasedGames)
                    .FirstOrDefaultAsync(u => u.Id == order.UserId, cancellationToken);

                if (user != null && request.GameId.HasValue)
                {
                    var game = await _context.Games.FindAsync(new object?[] { request.GameId.Value }, cancellationToken);
                    if (game != null)
                    {
                        user.PurchasedGames ??= new List<Models.Game>();
                        var alreadyPurchased = user.PurchasedGames.Any(g => g.Id == request.GameId.Value);
                        if (!alreadyPurchased)
                        {
                            user.PurchasedGames.Add(game);
                        }
                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            var orderItemProfile = OrderItemMappingProfile.ToProfile(orderItem);

            _logger.LogInformation("CreateOrderItem : Order item created successfully");         
            return Result<OrderItemViewProfile>.Success(orderItemProfile, "Order item created successfully.");
        }

    }
}
