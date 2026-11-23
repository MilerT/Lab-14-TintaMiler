using LAB10_TINTAMILER.DOMAIN.Entities;
using Microsoft.EntityFrameworkCore;

namespace LAB10_TINTAMILER.INFRASTRUCTURE.Context;

public class TicketeraDbContext : DbContext
{
    public TicketeraDbContext(DbContextOptions<TicketeraDbContext> options) : base(options)
    {
    }

    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Response> Responses => Set<Response>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Tabla roles
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("roles");
            entity.HasKey(r => r.RoleId);
            entity.Property(r => r.RoleId).HasColumnName("role_id");
            entity.Property(r => r.RoleName).HasColumnName("role_name").HasMaxLength(50).IsRequired();
            entity.HasIndex(r => r.RoleName).IsUnique();
        });

        // Tabla users
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(u => u.UserId);
            entity.Property(u => u.UserId).HasColumnName("user_id");
            entity.Property(u => u.Username).HasColumnName("username").HasMaxLength(100).IsRequired();
            entity.HasIndex(u => u.Username).IsUnique();
            entity.Property(u => u.PasswordHash).HasColumnName("password_hash").HasMaxLength(255).IsRequired();
            entity.Property(u => u.Email).HasColumnName("email").HasMaxLength(150);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp without time zone").HasDefaultValueSql("now()");
        });

        // Tabla user_roles (pivote muchos a muchos)
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("user_roles");
            entity.HasKey(ur => new { ur.UserId, ur.RoleId });
            entity.Property(ur => ur.UserId).HasColumnName("user_id");
            entity.Property(ur => ur.RoleId).HasColumnName("role_id");
            entity.Property(ur => ur.AssignedAt).HasColumnName("assigned_at").HasColumnType("timestamp without time zone").HasDefaultValueSql("now()");

            entity.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Tabla tickets
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("tickets");
            entity.HasKey(t => t.TicketId);
            entity.Property(t => t.TicketId).HasColumnName("ticket_id");
            entity.Property(t => t.UserId).HasColumnName("user_id").IsRequired();
            entity.Property(t => t.Title).HasColumnName("title").HasMaxLength(255).IsRequired();
            entity.Property(t => t.Description).HasColumnName("description").HasColumnType("text");
            entity.Property(t => t.Status).HasColumnName("status").HasMaxLength(20).IsRequired();
            entity.Property(t => t.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp without time zone").HasDefaultValueSql("now()");
            entity.Property(t => t.ClosedAt).HasColumnName("closed_at").HasColumnType("timestamp without time zone");

            entity.HasOne(t => t.User)
                .WithMany(u => u.Tickets)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Tabla responses
        modelBuilder.Entity<Response>(entity =>
        {
            entity.ToTable("responses");
            entity.HasKey(res => res.ResponseId);
            entity.Property(res => res.ResponseId).HasColumnName("response_id");
            entity.Property(res => res.TicketId).HasColumnName("ticket_id").IsRequired();
            entity.Property(res => res.ResponderId).HasColumnName("responder_id").IsRequired();
            entity.Property(res => res.Message).HasColumnName("message").HasColumnType("text").IsRequired();
            entity.Property(res => res.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp without time zone").HasDefaultValueSql("now()");

            entity.HasOne(res => res.Ticket)
                .WithMany(t => t.Responses)
                .HasForeignKey(res => res.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(res => res.Responder)
                .WithMany(u => u.Responses)
                .HasForeignKey(res => res.ResponderId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
