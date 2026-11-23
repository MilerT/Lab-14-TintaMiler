namespace LAB10_TINTAMILER.APPLICATION.DTOs.Reports;

/// <summary>
/// DTO de salida para el detalle de respuestas de un ticket (usado como segunda hoja del Reporte 2).
/// </summary>
public class TicketResponseReportDto
{
    public Guid ResponseId { get; set; }
    public Guid TicketId { get; set; }
    public string TituloTicket { get; set; } = string.Empty;
    public string Respondedor { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public DateTime FechaRespuesta { get; set; }
}

