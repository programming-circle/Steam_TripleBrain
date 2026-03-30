using MediatR;
using Steam_TripleBrain.CQRS.Query.AuthQuery;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Repositories.Interface;

namespace Steam_TripleBrain.CQRS.Handler
{
    public class GetProfileHandler : IRequestHandler<GetProfileQuery, ProfilesAcc>
    {
        private readonly IUserRepository _repo;
        public GetProfileHandler(IUserRepository repo)
        {
            _repo = repo;
        }
        public async Task<ProfilesAcc> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrWhiteSpace(request.Username))
                throw new ArgumentException("Username cannot be null or empty.");                     // Якщо по запросу не передано ім'я користувача, викидаємо виняток

            var profile = await _repo.GetByUsernameAsync(request.Username);                           // Викликаємо метод репозиторію для отримання профілю за ім'ям користувача

            if (profile == null)
                throw new KeyNotFoundException($"No profile found for username: {request.Username}"); // Якщо профіль не знайдено, викидаємо виняток

            // Map Profiles (entity) to ProfilesAcc (DTO)
            var result = new ProfilesAcc
            {
                Id = profile.Id,
                Username = profile.Username,
                PasswordHash = profile.PasswordHash,
                Role = profile.Role,
                Email = profile.Email,
                FullName = profile.FullName
            };

            return result;
        }
    }
}
