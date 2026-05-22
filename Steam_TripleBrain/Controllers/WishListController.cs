using MediatR;
using Microsoft.AspNetCore.Mvc;
using Steam_TripleBrain.CQRS.Command.Game;
using Steam_TripleBrain.CQRS.Command.WishList;
using Steam_TripleBrain.Data;

namespace Steam_TripleBrain.Controllers
{
    public class WishlistController : Controller
    {
        private readonly IMediator _mediatr;
        private readonly AppDbContext _context;
        private readonly ILogger<WishlistController> _logger;

        public WishlistController(IMediator mediatr, AppDbContext context, ILogger<WishlistController> logger)
        {
            _mediatr = mediatr;
            _context = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("create-wishlist")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateWishListCommand request)
        {
            var result = await _mediatr.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("update-wishlist")]
        public async Task<IActionResult> CreateAsync([FromBody] UpdateWishListCommand request)
        {
            var result = await _mediatr.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


    }
}
