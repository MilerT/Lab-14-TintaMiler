using LAB10_TINTAMILER.APPLICATION.DTOs.Tickets;
using MediatR;

namespace LAB10_TINTAMILER.APPLICATION.Queries.Tickets;

/// <summary>
/// Consulta CQRS para obtener un ticket por su identificador único (GUID).
/// Retorna el TicketDto con sus detalles o null si no existe.
/// </summary>
public record GetTicketByIdQuery(
    Guid Id
) : IRequest<TicketDto?>;
