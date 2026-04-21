using Steam_TripleBrain.CQRS.Command.OrderItems;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;
using System;
using System.Linq;
namespace Steam_TripleBrain.MappingProfiles
{
    public class OrderItemMappingProfile
    {
        OrderItemMappingProfile() { }

        public static OrderItem ToOrderItem(CreateOrderItemCommand cmd)
        {
            return new OrderItem
            {
                Id = cmd.Id == Guid.Empty ? Guid.NewGuid() : cmd.Id,
                OrderId = cmd.OrderId,
                GameId = cmd.GameId,
                //DLCId = cmd.DLCId,
                PriceOfItem = cmd.PriceOfItem
            };
        }

        public static OrderItemViewProfile ToProfile(OrderItem item)
        {
            return new OrderItemViewProfile
            {
                Id = item.Id,
                OrderId = item.OrderId,
                GameId = item.GameId,
                //DLCId = item.DLCId,
                PriceOfItem = item.PriceOfItem
            };
        }
    }
}
