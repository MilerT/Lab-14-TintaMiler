using LAB10_TINTAMILER.DOMAIN.Entities;

namespace LAB10_TINTAMILER.DOMAIN.Repositories;

/// <summary>
/// Contrato de repositorio para obtener datos de usuarios con sus tickets (Reporte 1).
/// Ubicado en Domain según el patrón Repository.
/// </summary>
public interface IUserReportRepository
{
    /// <summary>
    /// Retorna todos los usuarios con sus tickets y roles asociados.
    /// </summary>
    Task<IEnumerable<User>> GetUsersWithTicketsAsync(CancellationToken cancellationToken = default);
}

