namespace LAB10_TINTAMILER.DOMAIN.Entities;

public class Response
{
    public Guid ResponseId { get; set; }
    public Guid TicketId { get; set; }
    public Guid ResponderId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relaciones
    public Ticket Ticket { get; set; } = null!;
    public User Responder { get; set; } = null!;
}
