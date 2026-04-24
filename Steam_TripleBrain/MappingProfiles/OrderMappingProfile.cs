using Steam_TripleBrain.CQRS.Command.Order;
//using Steam_TripleBrain.CQRS.Command.Orders;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.MappingProfiles
{
    public class OrderMappingProfile
    {
        public OrderMappingProfile() { }

        public static Order ToOrder(CreateOrderCommand cmd)
        {
            var id = cmd.Id == Guid.Empty ? Guid.NewGuid() : cmd.Id;


            return new Order
            {
                Id = id,
                UserId = cmd.UserId,
                CreatedAt = DateTime.UtcNow,
                Items = cmd.Items?.Select(i => new OrderItem
                {
                    Id = i.Id == Guid.Empty ? Guid.NewGuid() : i.Id,
                    OrderId = id,
                    GameId = i.Items?.Select(i => i.GameId).FirstOrDefault(), //Could be problem , if FirstOrDefault, compilator could take only first item .
                    //DLCId = i.DLCId,
                    PriceOfItem = i.Items.Select(i => i.PriceOfItem).FirstOrDefault(),
                }).ToList() ?? new List<OrderItem>(),
                TotalPrice = cmd.TotalPrice,
            };

        }

        public static OrderViewProfile ToProfile(Order order)
        {
            var id = order.Id == Guid.Empty ? Guid.NewGuid() : order.Id;

            return new OrderViewProfile
            {
                Id = order.Id,
                UserId = order.UserId,
                CreatedAt = DateTime.UtcNow,
                Items = order.Items?.Select(i => new OrderItemViewProfile
                {
                    Id = i.Id == Guid.Empty ? Guid.NewGuid() : i.Id,
                    OrderId = id,
                    GameId = i.GameId,  
                    //DLCId = i.DLCId,
                    PriceOfItem = i.PriceOfItem,
                }).ToList() ?? new List<OrderItemViewProfile>(),
                TotalPrice = order.TotalPrice,
            };
        }
    }
}
