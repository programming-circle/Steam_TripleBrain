using MediatR;
using Microsoft.AspNetCore.Mvc;
using Steam_TripleBrain.CQRS.Command.Game;
using Steam_TripleBrain.CQRS.Command.OrderItem;
using Steam_TripleBrain.CQRS.Query.OrderItem;
using Steam_TripleBrain.Data;

namespace Steam_TripleBrain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : Controller
    {
        private readonly IMediator _mediatr;
        private readonly AppDbContext _context;
        private readonly ILogger<OrderItemController> _logger;
        public OrderItemController(IMediator mediatr, AppDbContext context, ILogger<OrderItemController> logger)
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
            _logger.LogInformation("Creating OrderItem started");
            var result = await _mediatr.Send(request);

            if (!result.IsSuccess)
            {
                _logger.LogInformation("OrderItem created unsuccessfully");
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Getting all OrderItems started");

            var result = await _mediatr.Send(new OrderItemGetAllQuery());

            if (!result.IsSuccess)
            {
                _logger.LogInformation("Getting all OrderItems unsuccessfully");
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] OrderItemDeleteByIdQuery request)
        {
            _logger.LogInformation("Deleting OrderItem started");
            var result = await _mediatr.Send(request);
            if (!result.IsSuccess)
            {
                _logger.LogInformation("OrderItem deleted unsuccessfully");
                return BadRequest(result);
            }
            _logger.LogInformation("OrderItem deleted successfully");
            return Ok(result);
        }
    }
}
