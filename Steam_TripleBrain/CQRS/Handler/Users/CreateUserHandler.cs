using MediatR;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Users;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.MappingProfiles;

namespace Steam_TripleBrain.CQRS.Handler.Users
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Result<User>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateUserHandler> _logger;

        public CreateUserHandler(AppDbContext context, ILogger<CreateUserHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Use mapping profile to build domain User from command
            var user = MappingProfile.ToUser(request);

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<User>.Success(user);
        }
    }
}
