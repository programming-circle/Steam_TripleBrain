using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Steam_TripleBrain.CQRS.Command.Game;
using Steam_TripleBrain.CQRS.Command.WishList;
using Steam_TripleBrain.Data;
using Steam_TripleBrain.MappingProfiles;
using Steam_TripleBrain.Models;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpGet("get-by-user")]
        public async Task<IActionResult> GetByUserAsync([FromQuery] string userId)
        {
            if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var parsedUserId))
            {
                return BadRequest(Result<WishListViewProfile>.Failure("UserId is required and must be a valid GUID."));
            }

            var wishList = await _context.WishLists
                .Include(w => w.WishGames)
                .FirstOrDefaultAsync(w => w.UserId == parsedUserId);

            if (wishList == null)
            {
                return Ok(Result<WishListViewProfile>.Success(null));
            }

            var profile = WishListMappingProfile.ToProfile(wishList);
            return Ok(Result<WishListViewProfile>.Success(profile));
        }

        [HttpPost("update-wishlist")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateWishListCommand request)
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
