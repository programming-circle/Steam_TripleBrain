
using Microsoft.AspNetCore.Identity;
using Steam_TripleBrain.Data;

namespace Auth.Seed
{
    public class IdentitySeed
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            
            using var scope = services.CreateScope();

            
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

            
            string[] roles = { "User", "Admin", "Manager" };

            
            foreach (var role in roles)
            {
                
                var exists = await roleManager.RoleExistsAsync(role);

                if (!exists)
                {
                    await roleManager.CreateAsync(new AppRole
                    {
                        Id = Guid.NewGuid(), 
                        Name = role         
                    });
                }
            }
        }
    }
}
