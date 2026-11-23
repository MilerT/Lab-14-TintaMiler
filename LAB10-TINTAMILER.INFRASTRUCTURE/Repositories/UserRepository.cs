using LAB10_TINTAMILER.DOMAIN.Entities;
using LAB10_TINTAMILER.DOMAIN.Interfaces;
using LAB10_TINTAMILER.INFRASTRUCTURE.Context;
using Microsoft.EntityFrameworkCore;

namespace LAB10_TINTAMILER.INFRASTRUCTURE.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(TicketeraDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbSet
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email != null && u.Email.ToLower() == email.ToLower());
    }

    public async Task<User?> GetByIdWithRolesAsync(Guid userId)
    {
        return await _dbSet
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }
}
