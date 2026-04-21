using System;
using System.Linq;
using Steam_TripleBrain.CQRS.Command.Game;
using Steam_TripleBrain.CQRS.Handler.Game;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.MappingProfiles
{
    public static class GameMappingProfile
    {
        static GameMappingProfile()
        {
            // AutoMapper configuration can be set up here if needed
        }
        public static Game ToGame(CreateGameCommand game )
        {
            // Ensure we have a valid id
            var id = game.Id == Guid.Empty ? Guid.NewGuid() : game.Id;

            return new Game()
            {
                Id = id,
                Name = game.Name,
                Poster = game.Poster == null ? null : new ImageUrl
                {
                    Id = game.Poster.Id == Guid.Empty ? Guid.NewGuid() : game.Poster.Id,
                    Url = game.Poster.Url
                },
                Images = game.Images?.Select(i => new ImageUrl
                {
                    Id = i.Id == Guid.Empty ? Guid.NewGuid() : i.Id,
                    Url = i.Url
                }).ToList(),
                Rating = game.Rating,
                Description = game.Description,
                // Genres: already domain types in the command, just make a defensive copy
                Genres = game.Genres?.Select(i => new Genre
                {
                    Id = i.Id == Guid.Empty ? Guid.NewGuid() : i.Id,
                    Name = i.Name
                }).ToList(),
                // Tags: convert to a list
                /*Tags = game.Tags?.Select(i => new Tag
                {
                    Id = i.Id == Guid.Empty ? Guid.NewGuid() : i.Id,
                    Name = i.Name
                }).ToList(),*/
                Price = game.Price,
                Discount = game.Discount,
                Developer = game.Developer,
                // DLCs: map each DTO to domain DLC; if types match this will copy the list
                /*DLCs = game.DLCs?.Select(d => new DLC
                {
                    Id = d.Id == Guid.Empty ? Guid.NewGuid() : d.Id,
                    Name = d.Name,
                    Price = d.Price,
                    Discount = d.Discount,
                    Description = d.Description,
                }).ToList()*/
            };
        }

        public static Game ToGame(UpdateGameCommand game)
        {
            // Ensure we have a valid id
            var id = game.Id == Guid.Empty ? Guid.NewGuid() : game.Id;

            return new Game()
            {
                Id = id,
                Name = game.Name,
                Poster = game.Poster == null ? null : new ImageUrl
                {
                    Id = game.Poster.Id == Guid.Empty ? Guid.NewGuid() : game.Poster.Id,
                    Url = game.Poster.Url
                },
                Images = game.Images?.Select(i => new ImageUrl
                {
                    Id = i.Id == Guid.Empty ? Guid.NewGuid() : i.Id,
                    Url = i.Url
                }).ToList(),
                Rating = game.Rating,
                Description = game.Description,
                // Genres: already domain types in the command, just make a defensive copy
                Genres = game.Genres?.Select(i => new Genre
                {
                    Id = i.Id == Guid.Empty ? Guid.NewGuid() : i.Id,
                    Name = i.Name
                }).ToList(),
                // Tags: convert to a list
                /*Tags = game.Tags?.Select(i => new Tag
                {
                    Id = i.Id == Guid.Empty ? Guid.NewGuid() : i.Id,
                    Name = i.Name
                }).ToList(),*/
                Price = game.Price,
                Discount = game.Discount,
                Developer = game.Developer,
                // DLCs: map each DTO to domain DLC; if types match this will copy the list
                /*DLCs = game.DLCs?.Select(d => new DLC
                {
                    Id = d.Id == Guid.Empty ? Guid.NewGuid() : d.Id,
                    Name = d.Name,
                    Price = d.Price,
                    Discount = d.Discount,
                    Description = d.Description,
                }).ToList()*/
            };
        }
        public static GameViewProfile ToProfile(Game game)
        {
            return new GameViewProfile()
            {
                Id = game.Id,
                Name = game.Name,
                Poster = game.Poster == null ? null : new ImageUrlViewProfile
                {
                    Id = game.Poster.Id == Guid.Empty ? Guid.NewGuid() : game.Poster.Id,
                    Url = game.Poster.Url
                },
                Images = game.Images?.Select(i => new ImageUrlViewProfile
                {
                    Id = i.Id == Guid.Empty ? Guid.NewGuid() : i.Id,
                    Url = i.Url
                }).ToList(),
                Rating = game.Rating,
                Description = game.Description,
                // Genres: already domain types in the command, just make a defensive copy
                Genres = game.Genres?.Select(i => new GenreViewProfile
                {
                    Id = i.Id == Guid.Empty ? Guid.NewGuid() : i.Id,
                    Name = i.Name
                }).ToList(),
                // Tags: convert to a list
                /*
                Tags = game.Tags?.Select(i => new TagViewProfile
                {
                    Id = i.Id == Guid.Empty ? Guid.NewGuid() : i.Id,
                    Name = i.Name
                }).ToList(), */
                Price = game.Price,
                Discount = game.Discount,
                Developer = game.Developer,
                CreatedAt = game.CreatedAt,
                // DLCs: map each DTO to domain DLC; if types match this will copy the list
                /*DLCs = game.DLCs?.Select(d => new DLCViewProfile
                {
                    Id = d.Id == Guid.Empty ? Guid.NewGuid() : d.Id,
                    Name = d.Name,
                    Price = d.Price,
                    Discount = d.Discount,
                    Description = d.Description,
                    CreatedAt = d.CreatedAt
                }).ToList()*/
            };
        }
    }
}
