using Steam_TripleBrain.Models;

namespace Steam_TripleBrain.Repositories.Interface
{
    public interface ITokenLogRepository
    {
        Task AddAsync(TokenLogs log);                                 //Adding new entry into DB
        Task<IEnumerable<TokenLogs>> GetByUserAsync(string username); // Getting all tokens bout one user
                                                                      //Using IEmurable for getting list with tokens.
        Task<TokenLogs> GetByTokenAsync(string token);                //Getting token by info
        Task RevokeAsync(string token);                               // Revoking token bout leaved user
                                                                     
    }
}
