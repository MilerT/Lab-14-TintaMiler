using LAB10_TINTAMILER.DOMAIN.Entities;

namespace LAB10_TINTAMILER.DOMAIN.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdWithRolesAsync(Guid userId);
}
