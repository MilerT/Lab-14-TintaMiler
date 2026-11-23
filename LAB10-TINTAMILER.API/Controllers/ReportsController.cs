using LAB10_TINTAMILER.APPLICATION.Interfaces;
using LAB10_TINTAMILER.APPLICATION.UseCases.Tickets.Queries;
using LAB10_TINTAMILER.APPLICATION.UseCases.Users.Queries;
using Microsoft.AspNetCore.Mvc;

namespace LAB10_TINTAMILER.API.Controllers;

/// <summary>
/// Controlador REST para la generación y descarga de reportes Excel.
/// Implementado como parte del Laboratorio 13 (ClosedXML + CQRS + Repository Pattern).
///
/// Endpoints:
///   GET /api/reports/users-with-tickets     → Reporte 1: Usuarios y sus Tickets (BD Real)
///   GET /api/reports/tickets-with-responses  → Reporte 2: Tickets con sus Respuestas (BD Real)
///   GET /api/reports/first-example          → Demostración Parte 2 (FirstExample de la guía)
/// </summary>
[ApiController]
[Route("api/reports")]
[Produces("application/json")]
public class ReportsController : ControllerBase
{
    private readonly GetUsersReportQuery _getUsersReportQuery;
    private readonly GetTicketsReportQuery _getTicketsReportQuery;
    private readonly IExcelReportService _excelReportService;
    private readonly ILogger<ReportsController> _logger;

    public ReportsController(
        GetUsersReportQuery getUsersReportQuery,
        GetTicketsReportQuery getTicketsReportQuery,
        IExcelReportService excelReportService,
        ILogger<ReportsController> logger)
    {
        _getUsersReportQuery = getUsersReportQuery;
        _getTicketsReportQuery = getTicketsReportQuery;
        _excelReportService = excelReportService;
        _logger = logger;
    }

    // ================================================================
    // REPORTE 1 – Usuarios con sus Tickets
    // GET /api/reports/users-with-tickets
    // ================================================================

    /// <summary>
    /// Genera y descarga el Reporte Excel de Usuarios con sus Tickets.
    /// </summary>
    /// <remarks>
    /// Consulta los usuarios registrados en la BD del Laboratorio 9 (ticketera_db)
    /// junto con el resumen de sus tickets (total, abiertos, en proceso, cerrados y roles).
    /// El archivo Excel generado incluye:
    /// - Hoja: "Usuarios con Tickets"
    /// - Encabezados con estilo profesional (ClosedXML)
    /// - Tabla Excel con CreateTable()
    /// - Colores alternos por fila
    /// - Ancho de columnas ajustado automáticamente
    /// </remarks>
    /// <param name="cancellationToken">Token de cancelación para la solicitud HTTP.</param>
    /// <returns>Archivo .xlsx descargable con datos reales de la BD.</returns>
    /// <response code="200">Excel generado correctamente.</response>
    /// <response code="204">La base de datos no contiene usuarios.</response>
    /// <response code="500">Error interno al generar el reporte.</response>
    [HttpGet("users-with-tickets")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUsersWithTicketsReport(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("[Lab13] Generando Reporte 1: Usuarios con Tickets");

            var fileBytes = await _getUsersReportQuery.ExecuteAsync(cancellationToken);

            if (fileBytes.Length == 0)
            {
                _logger.LogWarning("[Lab13] No se encontraron usuarios en la BD para el Reporte 1.");
                return NoContent();
            }

            var fileName = $"usuarios_tickets_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";

            _logger.LogInformation("[Lab13] Reporte 1 generado exitosamente: {FileName}", fileName);

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Lab13] Error al generar Reporte 1 (Usuarios con Tickets).");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Message = "Ocurrió un error al generar el reporte. Contacte al administrador." });
        }
    }

    // ================================================================
    // REPORTE 2 – Tickets con sus Respuestas
    // GET /api/reports/tickets-with-responses
    // ================================================================

    /// <summary>
    /// Genera y descarga el Reporte Excel de Tickets con sus Respuestas.
    /// </summary>
    /// <remarks>
    /// Consulta todos los tickets de la BD del Laboratorio 9 (ticketera_db)
    /// con sus respuestas asociadas y los datos del respondedor.
    /// El archivo Excel generado incluye:
    /// - Hoja 1: "Resumen de Tickets" – estado, usuario creador, fechas, total respuestas.
    /// - Hoja 2: "Detalle de Respuestas" – mensaje completo, respondedor, fecha.
    /// - Estilo profesional en ambas hojas con tablas Excel (CreateTable).
    /// - Color de fila según estado del ticket (abierto/en_proceso/cerrado).
    /// </remarks>
    /// <param name="cancellationToken">Token de cancelación para la solicitud HTTP.</param>
    /// <returns>Archivo .xlsx descargable con datos reales de la BD.</returns>
    /// <response code="200">Excel generado correctamente.</response>
    /// <response code="204">La base de datos no contiene tickets.</response>
    /// <response code="500">Error interno al generar el reporte.</response>
    [HttpGet("tickets-with-responses")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTicketsWithResponsesReport(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("[Lab13] Generando Reporte 2: Tickets con Respuestas");

            var fileBytes = await _getTicketsReportQuery.ExecuteAsync(cancellationToken);

            if (fileBytes.Length == 0)
            {
                _logger.LogWarning("[Lab13] No se encontraron tickets en la BD para el Reporte 2.");
                return NoContent();
            }

            var fileName = $"tickets_respuestas_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";

            _logger.LogInformation("[Lab13] Reporte 2 generado exitosamente: {FileName}", fileName);

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Lab13] Error al generar Reporte 2 (Tickets con Respuestas).");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Message = "Ocurrió un error al generar el reporte. Contacte al administrador." });
        }
    }

    // ================================================================
    // EJEMPLO DIDÁCTICO DEL LABORATORIO – PARTE 2: FirstExample
    // GET /api/reports/first-example
    // ================================================================

    /// <summary>
    /// Genera el archivo Excel básico siguiendo el Paso 1 de la Parte 2 de la guía (FirstExample).
    /// </summary>
    /// <remarks>
    /// Demostración pedagógica del método FirstExample con ClosedXML:
    /// Crea un XLWorkbook, añade una hoja 'Sample Sheet' y celdas A1='Hello World!', A2=123, A3=Fecha.
    /// </remarks>
    [HttpGet("first-example")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    public IActionResult GetFirstExample()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"FirstExample_{Guid.NewGuid():N}.xlsx");
        _excelReportService.FirstExample(tempFile);
        var bytes = System.IO.File.ReadAllBytes(tempFile);
        System.IO.File.Delete(tempFile);

        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FirstExample.xlsx");
    }
}

