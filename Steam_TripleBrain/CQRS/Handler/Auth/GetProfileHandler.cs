using MediatR;
using Steam_TripleBrain.CQRS.Query.AuthQuery;
using Steam_TripleBrain.CQRS.Query.AuthQuery;
using Steam_TripleBrain.Data;
using Microsoft.AspNetCore.Identity;

namespace Steam_TripleBrain.CQRS.Handler.Auth
{
    public class GetProfileHandler : IRequestHandler<GetProfileQuery, AppUser>
    {
        private readonly UserManager<AppUser> _userManager;
        public GetProfileHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUser> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrWhiteSpace(request.Username))
                throw new ArgumentException("Username cannot be null or empty.");

            var user = await _userManager.FindByNameAsync(request.Username) ?? await _userManager.FindByEmailAsync(request.Username);

            if (user == null)
                throw new KeyNotFoundException($"No profile found for username: {request.Username}");

            return user;
        }
    }
}
