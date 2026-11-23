using LAB10_TINTAMILER.APPLICATION.DTOs.Reports;
using LAB10_TINTAMILER.APPLICATION.Interfaces;
using LAB10_TINTAMILER.DOMAIN.Repositories;

namespace LAB10_TINTAMILER.APPLICATION.UseCases.Tickets.Queries;

/// <summary>
/// Query CQRS para obtener el Reporte 2: Tickets con sus Respuestas.
/// Coordina la obtención de datos vía ITicketReportRepository
/// y delega la generación del Excel a IExcelReportService.
/// Flujo: Controller → GetTicketsReportQuery → ITicketReportRepository → Repository → DbContext
///                                           → IExcelReportService → ExcelReportService (Infrastructure)
/// </summary>
public class GetTicketsReportQuery
{
    private readonly ITicketReportRepository _ticketReportRepository;
    private readonly IExcelReportService _excelReportService;

    public GetTicketsReportQuery(
        ITicketReportRepository ticketReportRepository,
        IExcelReportService excelReportService)
    {
        _ticketReportRepository = ticketReportRepository;
        _excelReportService = excelReportService;
    }

    /// <summary>
    /// Ejecuta la query: consulta tickets con respuestas y genera el archivo Excel.
    /// </summary>
    /// <returns>Bytes del archivo .xlsx generado con datos reales de la BD.</returns>
    public async Task<byte[]> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        // 1. Obtener datos reales de la BD vía repositorio (AsNoTracking, solo lectura)
        var tickets = await _ticketReportRepository.GetTicketsWithResponsesAsync(cancellationToken);
        var ticketList = tickets.ToList();

        // 2. Proyectar tickets a DTOs de reporte (hoja 1: resumen de tickets)
        var ticketDtos = ticketList.Select(t => new TicketReportDto
        {
            TicketId        = t.TicketId,
            UsuarioCreador  = t.User?.Username ?? "N/A",
            EmailCreador    = t.User?.Email,
            Titulo          = t.Title,
            Descripcion     = t.Description,
            Estado          = t.Status,
            FechaCreacion   = t.CreatedAt,
            FechaCierre     = t.ClosedAt,
            TotalRespuestas = t.Responses.Count
        }).ToList();

        // 3. Proyectar respuestas a DTOs de reporte (hoja 2: detalle de respuestas)
        var responseDtos = ticketList
            .SelectMany(t => t.Responses.Select(r => new TicketResponseReportDto
            {
                ResponseId     = r.ResponseId,
                TicketId       = t.TicketId,
                TituloTicket   = t.Title,
                Respondedor    = r.Responder?.Username ?? "N/A",
                Mensaje        = r.Message,
                FechaRespuesta = r.CreatedAt
            }))
            .ToList();

        // 4. Generar Excel con ClosedXML y retornar bytes
        return _excelReportService.GenerateTicketsWithResponsesReport(ticketDtos, responseDtos);
    }
}

