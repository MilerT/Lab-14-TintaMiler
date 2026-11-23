using LAB10_TINTAMILER.APPLICATION.DTOs.Tickets;
using MediatR;

namespace LAB10_TINTAMILER.APPLICATION.Commands.Tickets;

/// <summary>
/// Comando CQRS para la creación de un nuevo ticket de soporte.
/// Implementa IRequest retornando el DTO completo del ticket generado.
/// </summary>
public record CreateTicketCommand(
    Guid UserId,
    string Title,
    string? Description
) : IRequest<TicketDto>;
