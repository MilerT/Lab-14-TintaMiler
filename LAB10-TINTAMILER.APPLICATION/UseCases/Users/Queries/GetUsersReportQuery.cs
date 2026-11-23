using LAB10_TINTAMILER.APPLICATION.DTOs.Reports;
using LAB10_TINTAMILER.APPLICATION.Interfaces;
using LAB10_TINTAMILER.DOMAIN.Repositories;

namespace LAB10_TINTAMILER.APPLICATION.UseCases.Users.Queries;

/// <summary>
/// Query CQRS para obtener el Reporte 1: Usuarios con sus Tickets.
/// Coordina la obtención de datos vía IUserReportRepository
/// y delega la generación del Excel a IExcelReportService.
/// Flujo: Controller → GetUsersReportQuery → IUserReportRepository → Repository → DbContext
///                                          → IExcelReportService → ExcelReportService (Infrastructure)
/// </summary>
public class GetUsersReportQuery
{
    private readonly IUserReportRepository _userReportRepository;
    private readonly IExcelReportService _excelReportService;

    public GetUsersReportQuery(
        IUserReportRepository userReportRepository,
        IExcelReportService excelReportService)
    {
        _userReportRepository = userReportRepository;
        _excelReportService = excelReportService;
    }

    /// <summary>
    /// Ejecuta la query: consulta usuarios con tickets y genera el archivo Excel.
    /// </summary>
    /// <returns>Bytes del archivo .xlsx generado con datos reales de la BD.</returns>
    public async Task<byte[]> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        // 1. Obtener datos reales de la BD vía repositorio (AsNoTracking, solo lectura)
        var users = await _userReportRepository.GetUsersWithTicketsAsync(cancellationToken);

        // 2. Proyectar entidades a DTOs de reporte
        var reportData = users.Select(u => new UserReportDto
        {
            UserId    = u.UserId,
            Username  = u.Username,
            Email     = u.Email,
            CreatedAt = u.CreatedAt,
            TotalTickets       = u.Tickets.Count,
            TicketsAbiertos    = u.Tickets.Count(t => t.Status == "abierto"),
            TicketsEnProceso   = u.Tickets.Count(t => t.Status == "en_proceso"),
            TicketsCerrados    = u.Tickets.Count(t => t.Status == "cerrado"),
            Roles = string.Join(", ", u.UserRoles
                .Where(ur => ur.Role != null)
                .Select(ur => ur.Role.RoleName))
        }).ToList();

        // 3. Generar Excel con ClosedXML y retornar bytes
        return _excelReportService.GenerateUsersWithTicketsReport(reportData);
    }
}

