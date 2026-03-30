using MediatR;
using Microsoft.AspNetCore.Mvc;
using Steam_TripleBrain.CQRS.Command.Game;
using Steam_TripleBrain.CQRS.Handler.Game;
using Steam_TripleBrain.CQRS.Query.Game;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : Controller
    {
        private readonly IMediator _mediatr;
        private readonly AppDbContext _context;
        private readonly ILogger<GameController> _logger;
        private readonly CancellationToken _cancellationToken;

        public GameController(IMediator mediatr, AppDbContext context, ILogger<GameController> logger)
        {
            _mediatr = mediatr;
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("get-game-ById")]
        public async Task<IActionResult> GetById([FromBody] GetGameByIdQueryRequest request)
        {
            var result = await _mediatr.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("create-game")]
        public async Task<IActionResult> CreateAsync([FromBody]CreateGameCommand request)
        {
            var result = await _mediatr.Send(request);

            if(!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
