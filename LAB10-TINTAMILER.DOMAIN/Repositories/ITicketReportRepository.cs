using LAB10_TINTAMILER.DOMAIN.Entities;

namespace LAB10_TINTAMILER.DOMAIN.Repositories;

/// <summary>
/// Contrato de repositorio para obtener tickets con sus respuestas (Reporte 2).
/// Ubicado en Domain según el patrón Repository.
/// </summary>
public interface ITicketReportRepository
{
    /// <summary>
    /// Retorna todos los tickets con sus respuestas y los datos del usuario que los abrió.
    /// </summary>
    Task<IEnumerable<Ticket>> GetTicketsWithResponsesAsync(CancellationToken cancellationToken = default);
}

