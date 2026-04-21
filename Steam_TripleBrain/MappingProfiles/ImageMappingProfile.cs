using Steam_TripleBrain.CQRS.Command.ImageUrls;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;
using System;
using System.Linq;
namespace Steam_TripleBrain.MappingProfiles
{
    public class ImageMappingProfile
    { 
        static ImageMappingProfile()
        {

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

        public static ImageUrlViewProfile ToProfile(ImageUrl image)
        {
            return new ImageUrlViewProfile
            {
                Id = image.Id,
                Url = image.Url
            };
        }
    }
}
