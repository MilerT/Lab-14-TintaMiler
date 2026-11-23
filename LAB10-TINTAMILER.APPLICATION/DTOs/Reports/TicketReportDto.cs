namespace LAB10_TINTAMILER.APPLICATION.DTOs.Reports;

/// <summary>
/// DTO de salida para el Reporte 2: Tickets con sus Respuestas.
/// Representa una fila del reporte Excel de tickets.
/// </summary>
public class TicketReportDto
{
    public Guid TicketId { get; set; }
    public string UsuarioCreador { get; set; } = string.Empty;
    public string? EmailCreador { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaCierre { get; set; }
    public int TotalRespuestas { get; set; }
}

