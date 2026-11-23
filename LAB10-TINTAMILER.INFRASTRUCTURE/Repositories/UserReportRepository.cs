using LAB10_TINTAMILER.DOMAIN.Entities;
using LAB10_TINTAMILER.DOMAIN.Repositories;
using LAB10_TINTAMILER.INFRASTRUCTURE.Context;
using Microsoft.EntityFrameworkCore;

namespace LAB10_TINTAMILER.INFRASTRUCTURE.Repositories;

/// <summary>
/// Implementación concreta del repositorio de reportes de usuarios.
/// Accede a la BD mediante TicketeraDbContext existente (Laboratorio 9).
/// Usa AsNoTracking() para consultas de solo lectura (mejor rendimiento).
/// </summary>
public class UserReportRepository : IUserReportRepository
{
    private readonly TicketeraDbContext _context;

    public UserReportRepository(TicketeraDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<User>> GetUsersWithTicketsAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .Include(u => u.Tickets)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .OrderBy(u => u.Username)
            .ToListAsync(cancellationToken);
    }
}

