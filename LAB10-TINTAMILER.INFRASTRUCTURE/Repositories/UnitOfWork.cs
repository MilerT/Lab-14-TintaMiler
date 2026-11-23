using LAB10_TINTAMILER.DOMAIN.Entities;
using LAB10_TINTAMILER.DOMAIN.Interfaces;
using LAB10_TINTAMILER.INFRASTRUCTURE.Context;

namespace LAB10_TINTAMILER.INFRASTRUCTURE.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly TicketeraDbContext _context;
    private IUserRepository? _users;
    private ITicketRepository? _tickets;
    private IRepository<Role>? _roles;
    private IRepository<Response>? _responses;
    private IRepository<UserRole>? _userRoles;

    public UnitOfWork(TicketeraDbContext context)
    {
        _context = context;
    }

    public IUserRepository Users => _users ??= new UserRepository(_context);
    public ITicketRepository Tickets => _tickets ??= new TicketRepository(_context);
    public IRepository<Role> Roles => _roles ??= new Repository<Role>(_context);
    public IRepository<Response> Responses => _responses ??= new Repository<Response>(_context);
    public IRepository<UserRole> UserRoles => _userRoles ??= new Repository<UserRole>(_context);

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
