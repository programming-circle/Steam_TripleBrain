using Steam_TripleBrain.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Steam_TripleBrain.Repositories.Interface
{
    public interface IUserRepository
    {
        Task AddAsync(ProfilesAcc profile);                    //Adding new profile/account
        Task<ProfilesAcc> GetByUsernameAsync(string username); //Getting profiles by Username
        Task<bool> DeleteAsync(string username); //Deleting
        Task<IEnumerable<ProfilesAcc>> GetAllAsync(); //Getting all
    }
}
