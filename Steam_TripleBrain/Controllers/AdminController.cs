using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Steam_TripleBrain.Repositories.Interface;

using Steam_TripleBrain.Models;
using Microsoft.EntityFrameworkCore;

namespace Steam_TripleBrain.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] // доступ лише для адміністраторів
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepo;

        public AdminController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepo.GetAllAsync();
            return Ok(users);
        }

        [HttpPost("users/{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var result = await _userRepo.DeleteAsync(username);
            if (!result)
                return NotFound($"User {username} not found");

            return Ok($"User {username} deleted successfully");
        }
    }
}
