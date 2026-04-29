using MediatR;
using Microsoft.AspNetCore.Mvc;
using Steam_TripleBrain.CQRS.Command.Genre;
using Steam_TripleBrain.CQRS.Query.Genre;
using Steam_TripleBrain.Data;

namespace Steam_TripleBrain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : Controller
    {
        private readonly IMediator _mediatr;
        private readonly AppDbContext _context;
        private readonly ILogger<GenresController> _logger;
        public GenresController(IMediator mediatr, AppDbContext context, ILogger<GenresController> logger)
        {
            _mediatr = mediatr;
            _context = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("create-genre")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateGenreCommand request)
        {
            _logger.LogInformation("Creating Genre started");
            var result = await _mediatr.Send(request);
            if (!result.IsSuccess)
            {
                _logger.LogInformation("Genre created unsuccessfully");
                return BadRequest(result);
            }
            _logger.LogInformation("Genre created successfully");
            return Ok(result);
        }
        [HttpGet("get-genre-ById")]
        public async Task<IActionResult> GetGenreByIdAsync([FromQuery] GetGenreByIdQueryRequest request)
        {
            _logger.LogInformation("Getting Genre by ID started");
            var result = await _mediatr.Send(request);
            if (!result.IsSuccess)
            {
                _logger.LogInformation("Genre not found");
                return BadRequest(result);
            }
            _logger.LogInformation("Genre retrieved successfully");
            return Ok(result);
        }
        [HttpPost("update-genre")]
        public async Task<IActionResult> UpdateGenreAsync([FromBody] UpdateGenreCommand request)
        {
            _logger.LogInformation("Updating Genre started");
            var result = await _mediatr.Send(request);
            if (!result.IsSuccess)
            {
                _logger.LogInformation("Genre updated unsuccessfully");
                return BadRequest(result);
            }
            _logger.LogInformation("Genre updated successfully");
            return Ok(result);
        }
    }
}
