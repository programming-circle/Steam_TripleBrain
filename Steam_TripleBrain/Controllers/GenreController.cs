using MediatR;
using Microsoft.AspNetCore.Mvc;
using Steam_TripleBrain.CQRS.Command.Genres;
using Steam_TripleBrain.CQRS.Query.Genres;
using Steam_TripleBrain.Profiles;

namespace Steam_TripleBrain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : Controller
    {
        private readonly IMediator _mediator;

        public GenreController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllGenresQuery());
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetGenreByIdQuery { Id = id });
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateGenreCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGenreCommand command)
        {
            if (command == null || id != command.Id)
                return BadRequest("Invalid genre data");

            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteGenreCommand { Id = id });
            if (!result.IsSuccess)
                return BadRequest(result);

            return NoContent();
        }
    }
}
