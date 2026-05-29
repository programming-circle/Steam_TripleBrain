using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.MappingProfiles
{
    public static class ReviewMappingProfile
    {
        public static Models.Review ToModel(Profiles.Review profile)
        {
            return new Models.Review
            {
                Id = profile.Id,
                UserId = profile.UserId,
                GameId = profile.GameId,
                Rating = profile.Rating,
                Text = profile.Text,
                CreatedAt = profile.CreatedAt
            };
        }

        public static Profiles.Review ToProfile(Models.Review model)
        {
            return new Profiles.Review
            {
                Id = model.Id,
                UserId = model.UserId,
                GameId = model.GameId,
                Rating = model.Rating,
                Text = model.Text,
                CreatedAt = model.CreatedAt
            };
        }
    }
}
