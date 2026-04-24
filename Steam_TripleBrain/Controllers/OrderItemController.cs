using MediatR;
using Microsoft.AspNetCore.Mvc;
using Steam_TripleBrain.CQRS.Command.Game;
using Steam_TripleBrain.CQRS.Command.OrderItem;
using Steam_TripleBrain.Data;

namespace Steam_TripleBrain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : Controller
    {
        private readonly IMediator _mediatr;
        private readonly AppDbContext _context;
        private readonly ILogger<GameController> _logger;
        public OrderItemController(IMediator mediatr, AppDbContext context, ILogger<GameController> logger)
        {
            _mediatr = mediatr;
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("create-game")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateOrderItemCommand request)
        {
            _logger.LogInformation("### Creating OrderItem started");
            var result = await _mediatr.Send(request);

            if (!result.IsSuccess)
            {
                _logger.LogInformation("OrderItem created unsuccessfully");
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
