
using System;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.MappingProfiles
{
    public static class DLCMappingProfile
    {
        // Map a Game entity that represents a DLC to the DLCViewProfile
        public static DLCViewProfile ToProfile(Game dlcGame)
        {
            if (dlcGame == null) return null;

            return new DLCViewProfile
            {
                Id = dlcGame.Id,
                Name = dlcGame.Name ?? string.Empty,
                Price = dlcGame.Price,
                Discount = dlcGame.Discount,
                Description = dlcGame.Description ?? string.Empty,
                CreatedAt = dlcGame.CreatedAt,
                GameId = dlcGame.ParentGameId ?? Guid.Empty
            };
        }
    }
}
