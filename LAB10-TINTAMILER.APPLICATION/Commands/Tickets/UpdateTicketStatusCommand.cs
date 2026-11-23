using MediatR;

namespace LAB10_TINTAMILER.APPLICATION.Commands.Tickets;

/// <summary>
/// Comando CQRS para actualizar el estado de un ticket existente.
/// Devuelve true si fue actualizado exitosamente o false si el ticket no existe.
/// </summary>
public record UpdateTicketStatusCommand(
    Guid TicketId,
    string Status
) : IRequest<bool>;
