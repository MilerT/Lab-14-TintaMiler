using LAB10_TINTAMILER.DOMAIN.Entities;

namespace LAB10_TINTAMILER.DOMAIN.Interfaces;

public interface ITicketRepository : IRepository<Ticket>
{
    Task<IEnumerable<Ticket>> GetAllWithDetailsAsync();
    Task<Ticket?> GetByIdWithDetailsAsync(Guid ticketId);
    Task<IEnumerable<Ticket>> GetByUserIdAsync(Guid userId);
}
