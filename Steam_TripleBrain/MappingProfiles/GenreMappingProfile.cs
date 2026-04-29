using Steam_TripleBrain.CQRS.Command;
using Steam_TripleBrain.CQRS.Command.Genre;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;
using System;
using System.Linq;

namespace Steam_TripleBrain.MappingProfiles
{
    public class GenreMappingProfile
    {
        static GenreMappingProfile()
        {
            
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

        public static GenreViewProfile ToProfile(Genre genre)
        {
            return new GenreViewProfile
            {
                Id = genre.Id,
                Name = genre.Name
            };
        }
    }
}
