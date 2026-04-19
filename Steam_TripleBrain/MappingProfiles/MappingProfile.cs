using Steam_TripleBrain.CQRS.Command.Orders;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;
using Steam_TripleBrain.CQRS.Command.OrderItems;
using Steam_TripleBrain.CQRS.Command.Users;
using Steam_TripleBrain.CQRS.Command.DLCs;
using Steam_TripleBrain.CQRS.Command.Genres;
using Steam_TripleBrain.CQRS.Command.Tags;
using Steam_TripleBrain.CQRS.Command.Reviews;
using System;
using System.Linq;
using System.Collections.Generic;
using Steam_TripleBrain.CQRS.Command.ImageUrls;

namespace Steam_TripleBrain.MappingProfiles
{
    public static class MappingProfile
    {
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
                    GameId = i.GameId,
                    DLCId = i.DLCId,
                    PriceOfItem = i.PriceOfItem
                }).ToList() ?? new List<OrderItem>(),
                TotalPrice = cmd.Items?.Sum(i => i.PriceOfItem) ?? 0
            };
        }

        // Overload for Update command - preserve provided order id
        public static Order ToOrder(Steam_TripleBrain.CQRS.Command.Orders.UpdateOrderCommand cmd, Guid orderId)
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
                    DLCId = i.DLCId,
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
                    DLCId = i.DLCId,
                    PriceOfItem = i.PriceOfItem
                }).ToList() ?? new List<OrderItemViewProfile>()
            };
        }

        public static OrderItem ToOrderItem(CreateOrderItemCommand cmd)
        {
            return new OrderItem
            {
                Id = cmd.Id == Guid.Empty ? Guid.NewGuid() : cmd.Id,
                OrderId = cmd.OrderId,
                GameId = cmd.GameId,
                DLCId = cmd.DLCId,
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
                DLCId = item.DLCId,
                PriceOfItem = item.PriceOfItem
            };
        }

        // User mapping
        public static User ToUser(CreateUserCommand cmd)
        {
            return new User
            {
                Id = cmd.Id == Guid.Empty ? Guid.NewGuid() : cmd.Id,
                UserName = cmd.UserName,
                Email = cmd.Email,
                Password = cmd.Password,
                DateOfReg = DateTime.UtcNow
            };
        }

        public static User ToUser(UpdateUserCommand cmd)
        {
            return new User
            {
                Id = cmd.Id,
                UserName = cmd.UserName,
                Email = cmd.Email,
                Password = cmd.Password
            };
        }

        public static UserViewProfile ToProfile(User user)
        {
            return new UserViewProfile
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Password = user.Password ?? string.Empty,
                Icon = user.Icon == null ? null : new ImageUrlViewProfile { Id = user.Icon.Id, Url = user.Icon.Url },
                DateOfReg = user.DateOfReg,
                PurchasedGames = user.PurchasedGames ?? new List<Game>(),
                DLCs = user.DLCs ?? new List<DLC>()
            };
        }

        // DLC mapping
        public static DLC ToDLC(CreateDLCCommand cmd)
        {
            return new DLC
            {
                Id = cmd.Id == Guid.Empty ? Guid.NewGuid() : cmd.Id,
                Name = cmd.Name,
                Price = cmd.Price,
                Discount = cmd.Discount,
                Description = cmd.Description,
                Game = null // caller should resolve Game from DB when needed
            };
        }

        public static DLC ToDLC(UpdateDLCCommand cmd)
        {
            return new DLC
            {
                Id = cmd.Id,
                Name = cmd.Name,
                Price = cmd.Price,
                Discount = cmd.Discount,
                Description = cmd.Description,
                Game = null
            };
        }

        public static DLCViewProfile ToProfile(DLC dlc)
        {
            return new DLCViewProfile
            {
                Id = dlc.Id,
                Name = dlc.Name,
                Price = dlc.Price,
                Discount = dlc.Discount,
                Description = dlc.Description,
                GameId = dlc.Game?.Id ?? Guid.Empty
            };
        }

        // Genre mapping
        public static Genre ToGenre(CreateGenreCommand cmd)
        {
            return new Genre
            {
                Id = cmd.Id == Guid.Empty ? Guid.NewGuid() : cmd.Id,
                Name = cmd.Name
            };
        }

        public static Genre ToGenre(UpdateGenreCommand cmd)
        {
            return new Genre
            {
                Id = cmd.Id,
                Name = cmd.Name
            };
        }

        public static GenreViewProfile ToProfile(Genre genre)
        {
            return new GenreViewProfile
            {
                Id = genre.Id,
                Name = genre.Name
            };
        }

        // Tag mapping
        public static Tag ToTag(CreateTagCommand cmd)
        {
            return new Tag
            {
                Id = cmd.Id == Guid.Empty ? Guid.NewGuid() : cmd.Id,
                Name = cmd.Name
            };
        }

        public static Tag ToTag(UpdateTagCommand cmd)
        {
            return new Tag
            {
                Id = cmd.Id,
                Name = cmd.Name
            };
        }

        public static TagViewProfile ToProfile(Tag tag)
        {
            return new TagViewProfile
            {
                Id = tag.Id,
                Name = tag.Name
            };
        }

        // ImageUrl mapping
        public static ImageUrl ToImageUrl(CreateImageUrlCommand cmd)
        {
            return new ImageUrl
            {
                Id = cmd.Id == Guid.Empty ? Guid.NewGuid() : cmd.Id,
                Url = cmd.Url
            };
        }

        public static ImageUrl ToImageUrl(UpdateImageUrlCommand cmd)
        {
            return new ImageUrl
            {
                Id = cmd.Id,
                Url = cmd.Url
            };
        }

        public static ImageUrlViewProfile ToProfile(ImageUrl image)
        {
            return new ImageUrlViewProfile
            {
                Id = image.Id,
                Url = image.Url
            };
        }

        // WishList mapping
        public static WishList ToWishList(CQRS.Command.WishLists.CreateWishListCommand cmd)
        {
            var id = cmd.Id == Guid.Empty ? Guid.NewGuid() : cmd.Id;
            return new WishList
            {
                Id = id,
                UserId = cmd.UserId,
                WishGames = cmd.WishGames ?? new List<Models.Game>()
            };
        }

        public static WishList ToWishList(CQRS.Command.WishLists.UpdateWishListCommand cmd)
        {
            return new WishList
            {
                Id = cmd.Id,
                UserId = cmd.UserId,
                WishGames = cmd.WishGames ?? new List<Models.Game>()
            };
        }

        public static WishListViewProfile ToProfile(WishList list)
        {
            return new WishListViewProfile
            {
                Id = list.Id,
                UserId = list.UserId,
                WishGames = list.WishGames ?? new List<Game>()
            };
        }

        // Review mapping
        public static Models.Review ToReview(CreateReviewCommand cmd)
        {
            return new Models.Review
            {
                Id = cmd.Id == Guid.Empty ? Guid.NewGuid() : cmd.Id,
                UserId = cmd.UserId,
                GameId = cmd.GameId,
                Rating = cmd.Rating,
                Text = cmd.Text,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static Models.Review ToReview(UpdateReviewCommand cmd)
        {
            return new Models.Review
            {
                Id = cmd.Id,
                UserId = cmd.UserId,
                GameId = cmd.GameId,
                Rating = cmd.Rating,
                Text = cmd.Text,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static Profiles.Review ToProfile(Models.Review review)
        {
            return new Profiles.Review
            {
                Id = review.Id,
                UserId = review.UserId,
                GameId = review.GameId,
                Rating = review.Rating,
                Text = review.Text,
                CreatedAt = review.CreatedAt
            };
        }
    }
}
