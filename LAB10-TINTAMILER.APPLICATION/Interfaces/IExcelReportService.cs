using LAB10_TINTAMILER.APPLICATION.DTOs.Reports;

namespace LAB10_TINTAMILER.APPLICATION.Interfaces;

/// <summary>
/// Interfaz del servicio de generación de reportes Excel con ClosedXML.
/// Definida en Application para que los casos de uso puedan coordinar
/// la generación sin acoplarse a la implementación concreta (Infrastructure).
/// </summary>
public interface IExcelReportService
{
    /// <summary>
    /// Genera el Reporte 1: Usuarios con resumen de sus Tickets.
    /// Aplica estilos profesionales, tablas Excel y formato coherente.
    /// </summary>
    /// <returns>Bytes del archivo .xlsx listo para descarga.</returns>
    byte[] GenerateUsersWithTicketsReport(IEnumerable<UserReportDto> data);

    /// <summary>
    /// Genera el Reporte 2: Tickets con sus Respuestas.
    /// Incluye dos hojas: resumen de tickets y detalle de respuestas.
    /// </summary>
    /// <returns>Bytes del archivo .xlsx listo para descarga.</returns>
    byte[] GenerateTicketsWithResponsesReport(
        IEnumerable<TicketReportDto> tickets,
        IEnumerable<TicketResponseReportDto> responses);

    // =========================================================================
    // MÉTODOS DE DEMOSTRACIÓN DEL LABORATORIO 13 (GUÍA DOCENTE: PARTES 2, 3, 4 Y 5)
    // =========================================================================

    /// <summary>
    /// PARTE 2: Crear un archivo Excel con ClosedXML (FirstExample)
    /// Crea un XLWorkbook, añade una hoja y celdas básicas (ID, Nombre, Edad).
    /// </summary>
    string FirstExample(string outputPath = "");

    /// <summary>
    /// PARTE 3: Modificar un archivo Excel existente
    /// Abre un XLWorkbook desde ruta, modifica una celda y guarda con Save().
    /// </summary>
    void ModifyExcelExample(string filePath);

    /// <summary>
    /// PARTE 4: Crear tablas en Excel con CreateTable
    /// </summary>
    string CreateTableExample(string outputPath = "");

    /// <summary>
    /// PARTE 5: Aplicar estilos a un archivo Excel
    /// </summary>
    string ApplyStylesExample(string outputPath = "");
}

