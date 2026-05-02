using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Order;
using Steam_TripleBrain.CQRS.Command.OrderItem;
using Steam_TripleBrain.CQRS.Handler.Game;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;
using Steam_TripleBrain.Services;

namespace Steam_TripleBrain.CQRS.Handler.Order
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Result<OrderViewProfile>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateOrderHandler> _logger;
        private readonly ITokenService _tokenService;

        public CreateOrderHandler(AppDbContext context, ILogger<CreateOrderHandler> logger, ITokenService tokenService)
        {
            _context = context;
            _logger = logger;
            _tokenService = tokenService;
        }

        public async Task<Result<OrderViewProfile>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating Order");
            _logger.LogInformation("### CreateOrder: decoding user token");
            var decodeResult = await _tokenService.DecodeTokenFromHeaders();
            if(!decodeResult.IsSuccess)
            {
                _logger.LogError("### CreateOrder: Error, DecodeToken failed {ErrorMessage}", decodeResult.ErrorMessage);
                return new() { IsSuccess = decodeResult.IsSuccess , ErrorMessage = decodeResult.ErrorMessage , statusCode = decodeResult.statusCode};
            }
            if(decodeResult == null )
            {
                _logger.LogError("### CreateOrder: Error, Decode Token Result is empty");
                return new() { IsSuccess = false, ErrorMessage = "Decode Result is empty", statusCode = 204 };
            }
            var user = decodeResult.Data;
            request.UserId = user.Id;
            var exists = await _context.Orders.AnyAsync(g => g.Id == request.Id, cancellationToken);
            if (exists)
            {
                _logger.LogInformation("CreateOrder: object with this allready exists");
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
