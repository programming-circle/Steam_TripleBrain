using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Services.Interface;
using LoginRequest = Steam_TripleBrain.Models.LoginRequest;

namespace Steam_TripleBrain.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ProfilesAcc profile)
        {
            var result = await _authService.RegisterAsync(profile); // Виклик методу реєстрації з сервісу
            if (!result)
                return BadRequest("Registration failed");

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password)) // Валідація вхідних даних
                return BadRequest("Username and password are required"); // Перевірка на null та порожні рядки

            var token = await _authService.LoginAsync(request.Username, request.Password); // Виклик методу логіну з сервісу
            if (token == null)
                return Unauthorized("Invalid credentials"); // Якщо токен не отримано, повертаємо статус 401 Unauthorized

            return Ok(new { Token = token }); // Повертаємо токен у відповіді
        }
    }

}
