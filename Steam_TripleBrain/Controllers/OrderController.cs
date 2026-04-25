using MediatR;
using Microsoft.AspNetCore.Mvc;
using Steam_TripleBrain.CQRS.Command.Order;
using Steam_TripleBrain.CQRS.Command.OrderItem;
using Steam_TripleBrain.Data;

namespace Steam_TripleBrain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IMediator _mediatr;
        private readonly AppDbContext _context;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IMediator mediatr, AppDbContext context, ILogger<OrderController> logger)
        {
            _mediatr = mediatr;
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateOrderCommand request)
        {
            _logger.LogInformation("#### Creating Order started");
            var result = await _mediatr.Send(request);

            if (!result.IsSuccess)
            {
                _logger.LogInformation("#### OrderItem created unsuccessfully");
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
