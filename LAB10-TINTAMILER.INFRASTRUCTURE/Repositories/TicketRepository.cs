using LAB10_TINTAMILER.DOMAIN.Entities;
using LAB10_TINTAMILER.DOMAIN.Interfaces;
using LAB10_TINTAMILER.INFRASTRUCTURE.Context;
using Microsoft.EntityFrameworkCore;

namespace LAB10_TINTAMILER.INFRASTRUCTURE.Repositories;

public class TicketRepository : Repository<Ticket>, ITicketRepository
{
    public TicketRepository(TicketeraDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Ticket>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .Include(t => t.User)
            .Include(t => t.Responses)
                .ThenInclude(r => r.Responder)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<Ticket?> GetByIdWithDetailsAsync(Guid ticketId)
    {
        return await _dbSet
            .Include(t => t.User)
            .Include(t => t.Responses)
                .ThenInclude(r => r.Responder)
            .FirstOrDefaultAsync(t => t.TicketId == ticketId);
    }

    public async Task<IEnumerable<Ticket>> GetByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Include(t => t.User)
            .Include(t => t.Responses)
                .ThenInclude(r => r.Responder)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }
}
