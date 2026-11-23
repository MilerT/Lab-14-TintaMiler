using MediatR;

namespace LAB10_TINTAMILER.APPLICATION.Commands.Tickets;

/// <summary>
/// Comando CQRS para la eliminación de un ticket por su identificador único.
/// Retorna true si fue eliminado correctamente, o false si no fue encontrado.
/// </summary>
public record DeleteTicketCommand(
    Guid TicketId
) : IRequest<bool>;
