using LAB10_TINTAMILER.DOMAIN.Entities;
using LAB10_TINTAMILER.DOMAIN.Repositories;
using LAB10_TINTAMILER.INFRASTRUCTURE.Context;
using Microsoft.EntityFrameworkCore;

namespace LAB10_TINTAMILER.INFRASTRUCTURE.Repositories;

/// <summary>
/// Implementación concreta del repositorio de reportes de tickets.
/// Accede a la BD mediante TicketeraDbContext existente (Laboratorio 9).
/// Usa AsNoTracking() para consultas de solo lectura (mejor rendimiento).
/// </summary>
public class TicketReportRepository : ITicketReportRepository
{
    private readonly TicketeraDbContext _context;

    public TicketReportRepository(TicketeraDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Ticket>> GetTicketsWithResponsesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Tickets
            .AsNoTracking()
            .Include(t => t.User)
            .Include(t => t.Responses)
                .ThenInclude(r => r.Responder)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}

