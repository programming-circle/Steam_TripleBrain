using Steam_TripleBrain.CQRS.Command.Orders;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.MappingProfiles
{
    public class OrderMappingProfile
    {
        OrderMappingProfile() { }

        public static Order ToOrder(UpdateOrderCommand cmd, Guid orderId)
        {
            var id = orderId == Guid.Empty ? (cmd.Id == Guid.Empty ? Guid.NewGuid() : cmd.Id) : orderId;

            return new Order
            {
                Id = id,
                UserId = cmd.UserId,
                CreatedAt = DateTime.UtcNow,
                Items = cmd.Items?.Select(i => new OrderItem
                {
                    Id = i.Id == Guid.Empty ? Guid.NewGuid() : i.Id,
                    OrderId = id,
                    GameId = i.GameId,
                    //DLCId = i.DLCId,
                    PriceOfItem = i.PriceOfItem
                }).ToList() ?? new List<OrderItem>(),
                TotalPrice = cmd.Items?.Sum(i => i.PriceOfItem) ?? 0
            };

        }

        public static OrderViewProfile ToProfile(Order order)
        {
            return new OrderViewProfile
            {
                Id = order.Id,
                UserId = order.UserId,
                CreatedAt = order.CreatedAt,
                TotalPrice = order.TotalPrice,
                Items = order.Items?.Select(i => new OrderItemViewProfile
                {
                    Id = i.Id,
                    OrderId = i.OrderId,
                    GameId = i.GameId,
                    //DLCId = i.DLCId,
                    PriceOfItem = i.PriceOfItem
                }).ToList() ?? new List<OrderItemViewProfile>()
            };
        }
    }
}
