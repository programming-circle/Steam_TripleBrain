using Steam_TripleBrain.CQRS.Command.WishList;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.MappingProfiles
{
    public class WishListMappingProfile
    {
        public static WishList ToWishList(CreateWishListCommand cmd)
        {
            var id = cmd.Id == Guid.Empty ? Guid.NewGuid() : cmd.Id;
            return new WishList
            {
                Id = id,
                UserId = cmd.UserId,
                WishGames = cmd.WishGames?.Select(g => new Game
                {
                    Id = g.Id,
                    Name = g.Name,
                    Poster = g.Poster,
                    Images = g.Images,
                    Rating = g.Rating,
                    Description = g.Description,
                    Genres = g.Genres?.Select( i => new Genre
                    {
                        Id = i.Id == Guid.Empty ? Guid.NewGuid() : i.Id,
                        Name = i.Name
                    }).ToList(),
                    Price = g.Price,
                    Discount = g.Discount,
                    Developer = g.Developer,

                }).ToList()
            };
        }

        public static WishListViewProfile ToProfile(WishList list)
        {
            var id = list.Id == Guid.Empty ? Guid.NewGuid() : list.Id;
            return new WishListViewProfile
            {
                Id = id,
                UserId = list.UserId,
                WishGames = list.WishGames?.Select(g => new GameViewProfile
                {
                    Id = g.Id,
                    Name = g.Name,
                    Poster = g.Poster,
                    Images = g.Images,
                    Rating = g.Rating,
                    Description = g.Description,
                    Genres = g.Genres?.Select(i => new GenreViewProfile
                    {
                        Id = i.Id == Guid.Empty ? Guid.NewGuid() : i.Id,
                        Name = i.Name
                    }).ToList(),
                    Price = g.Price,
                    Discount = g.Discount,
                    Developer = g.Developer,

                }).ToList()
            };
        }
    }
}
