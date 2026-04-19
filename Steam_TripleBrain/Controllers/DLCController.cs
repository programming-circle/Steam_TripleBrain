using MediatR;
using Microsoft.AspNetCore.Mvc;
using Steam_TripleBrain.CQRS.Command.DLCs;
using Steam_TripleBrain.CQRS.Query.DLCs;

namespace Steam_TripleBrain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DLCController : Controller
    {
        private readonly IMediator _mediator;

        public DLCController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllDLCsQuery());
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetDLCByIdQuery { Id = id });
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateDLCCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDLCCommand command)
        {
            if (command == null || id != command.Id)
                return BadRequest("Invalid DLC data");

            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteDLCCommand { Id = id });
            if (!result.IsSuccess)
                return BadRequest(result);

            return NoContent();
        }
    }
}
