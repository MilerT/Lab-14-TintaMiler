using LAB10_TINTAMILER.APPLICATION.DTOs.Tickets;
using MediatR;

namespace LAB10_TINTAMILER.APPLICATION.Queries.Tickets;

/// <summary>
/// Consulta CQRS para obtener todos los tickets creados por un usuario específico.
/// </summary>
public record GetTicketsByUserQuery(
    Guid UserId
) : IRequest<IEnumerable<TicketDto>>;
