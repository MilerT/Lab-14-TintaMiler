using LAB10_TINTAMILER.APPLICATION.DTOs.Responses;
using MediatR;

namespace LAB10_TINTAMILER.APPLICATION.Commands.Tickets;

/// <summary>
/// Comando CQRS para agregar una respuesta técnica a un ticket de soporte.
/// </summary>
public record AddResponseCommand(
    Guid TicketId,
    Guid ResponderId,
    string Message
) : IRequest<ResponseDto?>;
