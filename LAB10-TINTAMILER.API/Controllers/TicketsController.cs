using System.Security.Claims;
using LAB10_TINTAMILER.APPLICATION.Commands.Tickets;
using LAB10_TINTAMILER.APPLICATION.DTOs.Responses;
using LAB10_TINTAMILER.APPLICATION.DTOs.Tickets;
using LAB10_TINTAMILER.APPLICATION.Queries.Tickets;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LAB10_TINTAMILER.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Requiere token JWT para todos los endpoints
public class TicketsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TicketsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lista todos los tickets existentes con sus respuestas y usuarios creadores mediante CQRS Query.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TicketDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var tickets = await _mediator.Send(new GetAllTicketsQuery(), cancellationToken);
        return Ok(tickets);
    }

    /// <summary>
    /// Obtiene el detalle de un ticket específico por su identificador mediante CQRS Query.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TicketDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var ticket = await _mediator.Send(new GetTicketByIdQuery(id), cancellationToken);
        if (ticket == null)
        {
            return NotFound(new { message = $"No se encontró el ticket con id {id}" });
        }
        return Ok(ticket);
    }

    /// <summary>
    /// Lista los tickets creados por un usuario específico mediante CQRS Query.
    /// </summary>
    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<TicketDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUser(Guid userId, CancellationToken cancellationToken)
    {
        var tickets = await _mediator.Send(new GetTicketsByUserQuery(userId), cancellationToken);
        return Ok(tickets);
    }

    /// <summary>
    /// Crea un nuevo ticket asociado al usuario autenticado mediante CQRS Command.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TicketDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateTicketDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized(new { message = "Identificador de usuario inválido en el token." });
        }

        var command = new CreateTicketCommand(userId, dto.Title, dto.Description);
        var created = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.TicketId }, created);
    }

    /// <summary>
    /// Actualiza el estado de un ticket ('abierto', 'en_proceso', 'cerrado') mediante CQRS Command.
    /// </summary>
    [HttpPut("{id:guid}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateTicketStatusDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = new UpdateTicketStatusCommand(id, dto.Status);
        var updated = await _mediator.Send(command, cancellationToken);
        if (!updated)
        {
            return NotFound(new { message = $"No se encontró el ticket con id {id}" });
        }

        return NoContent();
    }

    /// <summary>
    /// Elimina un ticket existente mediante CQRS Command.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteTicketCommand(id);
        var deleted = await _mediator.Send(command, cancellationToken);
        if (!deleted)
        {
            return NotFound(new { message = $"No se encontró el ticket con id {id}" });
        }

        return NoContent();
    }

    /// <summary>
    /// Agrega una respuesta a un ticket por parte del usuario autenticado mediante CQRS Command.
    /// </summary>
    [HttpPost("{id:guid}/responses")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddResponse(Guid id, [FromBody] CreateResponseDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out var responderId))
        {
            return Unauthorized(new { message = "Identificador de usuario inválido en el token." });
        }

        var command = new AddResponseCommand(id, responderId, dto.Message);
        var response = await _mediator.Send(command, cancellationToken);
        if (response == null)
        {
            return NotFound(new { message = $"No se encontró el ticket con id {id} o el usuario no existe." });
        }

        return StatusCode(StatusCodes.Status201Created, response);
    }
}
