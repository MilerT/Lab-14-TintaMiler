namespace LAB10_TINTAMILER.DOMAIN.Entities;

public class Role
{
    public Guid RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;

    // Relaciones
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
