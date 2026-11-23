using LAB10_TINTAMILER.APPLICATION.DTOs.Tickets;
using MediatR;

namespace LAB10_TINTAMILER.APPLICATION.Queries.Tickets;

/// <summary>
/// Consulta CQRS para obtener el listado completo de tickets registrados.
/// Retorna la colección de TicketDto con usuarios y respuestas asociadas.
/// </summary>
public record GetAllTicketsQuery() : IRequest<IEnumerable<TicketDto>>;
