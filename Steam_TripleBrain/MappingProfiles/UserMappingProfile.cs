
using Steam_TripleBrain.CQRS.Command.Auth;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.MappingProfiles
{
    public static class UserMappingProfile
    {
        // Map RegisterRequest (API model) to domain User model
        public static User ToUser(RegisterRequest rr)
        {
            var id = Guid.NewGuid();
            return new User
            {
                Id = id,
                UserName = rr.Username,
                Email = rr.Email,
                Password = rr.Password,
                CreatedAt = DateTime.UtcNow
            };
        }

        // Map RegisterCommand (CQRS) to Identity AppUser
        public static AppUser ToAppUser(RegisterCommand cmd)
        {
            var id = cmd.Id == Guid.Empty ? Guid.NewGuid() : cmd.Id;
            return new AppUser
            {
                Id = id,
                UserName = cmd.Username,
                Email = cmd.Email,
                EmailConfirmed = false
            };
        }
    }
}
