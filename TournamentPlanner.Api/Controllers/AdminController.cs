using Microsoft.AspNetCore.Mvc;
using TournamentPlanner.Application;
using TournamentPlanner.Application.DTOs;
using TournamentPlanner.Mediator;

namespace TournamentPlanner.Api.Controllers
{
    [ApiController]
    [Route("/api/admins")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AdminDto>))]
        public async Task<ActionResult<IEnumerable<AdminDto>>> GetAllAdmin([FromQuery] string? name)
        {
            var getAllAdminRequest = new GetAllAdminRequest(name);
            var adminsDto = await _mediator.Send(getAllAdminRequest);
            return Ok(adminsDto);
        }

        [HttpGet("{id}", Name = nameof(GetAdminById))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AdminDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAdminById([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest("Id can not be negative");
            }

            var getAdminByIdRequest = new GetAdminByIdRequest(id);
            var adminDto = await _mediator.Send(getAdminByIdRequest);
            if (adminDto == null)
            {
                return NotFound();
            }

            return Ok(adminDto);

        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AdminDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAdmin([FromBody] AddAdminDto addAdminDto)
        {
            var addAdminRequest = new AddAdminRequest(addAdminDto);
            var player = await _mediator.Send(addAdminRequest);
            if (player == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetAdminById), new { id = player.Id }, player);
        }

    }
}