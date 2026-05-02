using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Models;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Steam_TripleBrain.CQRS.Command.User;

namespace Steam_TripleBrain.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<UsersController> _logger;

        public UsersController(AppDbContext db, UserManager<AppUser> userManager, ILogger<UsersController> logger)
        {
            _db = db;
            _userManager = userManager;
            _logger = logger;
        }
        
        //Method to get information about user.
        [HttpGet("account")]
        public async Task<IActionResult> Me()
        {
            _logger.LogInformation("#### Getting info about account started");
            var idRaw = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(idRaw, out var userId))
            {
                _logger.LogInformation("#### Me: Token empty or incorrect or get old");
                return Unauthorized("Invalid token.");
            }
                

            var user = await _db.Users
                .Include(u => u.Icon)
                .Include(u => u.PurchasedGames)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            { 
                _logger.LogInformation("#### Me: User not found");
                return NotFound("User profile not found."); }

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                IconUrl = user.Icon ?? null,
                user.CreatedAt,
                PurchasedGames = user.PurchasedGames?.Select(g => new { g.Id, g.Name })
            });
        }


        [HttpPut("update-account")]
        public async Task<IActionResult> UpdateMe([FromBody] UpdateMeRequest request)
        {
            _logger.LogInformation("#### UpdateMe start working");
            if (request == null){
                _logger.LogInformation("#### UpdateMe: Request body is required.");
                return BadRequest("Request body is required.");
            }

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var idRaw = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(idRaw, out var userId))
                return Unauthorized("Invalid token.");

            var appUser = await _userManager.FindByIdAsync(userId.ToString());
            if (appUser == null){
                _logger.LogInformation("#### UpdateMe: ");
                return NotFound("Identity user not found.");
            }

            var user = await _db.Users
                .Include(u => u.Icon)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null){
                _logger.LogInformation("#### Update me: User not found");
                return NotFound("User profile not found.");
            }

            // Update username/email in both Identity and domain User table (keep in sync)
            if (!string.IsNullOrWhiteSpace(request.Username) && request.Username != user.UserName)
            {
                var exists = await _db.Users.AnyAsync(u => u.UserName == request.Username && u.Id != userId);
                if (exists){
                    _logger.LogInformation("#### Update Me: Username allready exist.");
                    return Conflict(new { message = "Пользователь с таким именем уже существует." });
                }

                user.UserName = request.Username;
                appUser.UserName = request.Username;
            }

            if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != user.Email)
            {
                var exists = await _db.Users.AnyAsync(u => u.Email == request.Email && u.Id != userId);
                if (exists){
                    _logger.LogInformation("#### Update Me: Email allready taken");
                    return Conflict(new { message = "Пользователь с таким email уже существует." });
                }

                user.Email = request.Email;
                appUser.Email = request.Email;
            }

            //if (request.IconUrl != null)
            //{
            //    if (string.IsNullOrWhiteSpace(request.IconUrl))
            //    {
            //        user.Icon = null;
            //    }
            //    else
            //    {
            //        if (user.Icon == null)
            //        {
            //            user.Icon = new ImageUrl { Id = Guid.NewGuid(), Url = request.IconUrl };
            //            _db.ImageUrls.Add(user.Icon);
            //        }
            //        else
            //        {
            //            user.Icon.Url = request.IconUrl;
            //        }
            //    }
            //}

            var identityResult = await _userManager.UpdateAsync(appUser);
            if (!identityResult.Succeeded)
            {
                _logger.LogWarning("#### UpdateMe: Identity update failed: {errors}", string.Join(',', identityResult.Errors.Select(e => e.Description)));
                return BadRequest(new { message = "Не удалось обновить пользователя (Identity)." });
            }

            await _db.SaveChangesAsync();

            return await Me();
        }
    }
}

