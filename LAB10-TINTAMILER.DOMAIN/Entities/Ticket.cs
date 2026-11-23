namespace LAB10_TINTAMILER.DOMAIN.Entities;

public class Ticket
{
    public Guid TicketId { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = "abierto"; // 'abierto', 'en_proceso', 'cerrado'
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ClosedAt { get; set; }

    // Relaciones
    public User User { get; set; } = null!;
    public ICollection<Response> Responses { get; set; } = new List<Response>();
}
