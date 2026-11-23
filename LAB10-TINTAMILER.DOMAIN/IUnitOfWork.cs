using LAB10_TINTAMILER.DOMAIN.Entities;

namespace LAB10_TINTAMILER.DOMAIN.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    ITicketRepository Tickets { get; }
    IRepository<Role> Roles { get; }
    IRepository<Response> Responses { get; }
    IRepository<UserRole> UserRoles { get; }

    Task<int> CommitAsync();
}
