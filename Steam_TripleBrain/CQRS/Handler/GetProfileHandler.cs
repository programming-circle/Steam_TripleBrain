using MediatR;
using Steam_TripleBrain.CQRS.Query;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Repositories.Interface;

namespace Steam_TripleBrain.CQRS.Handler
{
    public class GetProfileHandler : IRequestHandler<GetProfileQuery, Profiles>
    {
        private readonly IUserRepository _repo;
        public GetProfileHandler(IUserRepository repo) 
        {
            _repo = repo;
        }

        public async Task<Profiles> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrWhiteSpace(request.Username))
                throw new ArgumentException("Username cannot be null or empty.");                     // Якщо по запросу не передано ім'я користувача, викидаємо виняток

            var profile = await _repo.GetByUsernameAsync(request.Username);                           // Викликаємо метод репозиторію для отримання профілю за ім'ям користувача

            if (profile == null)
                throw new KeyNotFoundException($"No profile found for username: {request.Username}"); // Якщо профіль не знайдено, викидаємо виняток

            return profile;                                                                           // Повертаємо знайдений профіль
        }
    }
}
