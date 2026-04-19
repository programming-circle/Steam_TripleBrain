using MediatR;
using Microsoft.AspNetCore.Mvc;
using Steam_TripleBrain.CQRS.Command.WishLists;
using Steam_TripleBrain.CQRS.Query.WishLists;

namespace Steam_TripleBrain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : Controller
    {
        private readonly IMediator _mediator;

        public WishListController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllWishListsQuery());
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetWishListByIdQuery { Id = id });
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateWishListCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWishListCommand command)
        {
            if (command == null || id != command.Id)
                return BadRequest("Invalid wishlist data");

            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteWishListCommand { Id = id });
            if (!result.IsSuccess)
                return BadRequest(result);

            return NoContent();
        }
    }
}
