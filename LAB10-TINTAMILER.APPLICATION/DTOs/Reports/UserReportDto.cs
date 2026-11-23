namespace LAB10_TINTAMILER.APPLICATION.DTOs.Reports;

/// <summary>
/// DTO de salida para el Reporte 1: Usuarios con sus Tickets.
/// Representa una fila del reporte Excel de usuarios.
/// </summary>
public class UserReportDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public int TotalTickets { get; set; }
    public int TicketsAbiertos { get; set; }
    public int TicketsEnProceso { get; set; }
    public int TicketsCerrados { get; set; }
    public string Roles { get; set; } = string.Empty;
}

