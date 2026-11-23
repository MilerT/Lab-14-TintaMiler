using ClosedXML.Excel;
using LAB10_TINTAMILER.APPLICATION.DTOs.Reports;
using LAB10_TINTAMILER.APPLICATION.Interfaces;

namespace LAB10_TINTAMILER.INFRASTRUCTURE.Services;

/// <summary>
/// Implementación concreta del servicio de generación de reportes Excel con ClosedXML.
/// Ubicada en Infrastructure, tal como indica la arquitectura de referencia.
///
/// Implementa las cinco prácticas del laboratorio:
///   - Parte 1: ClosedXML instalado como paquete NuGet en Infrastructure.
///   - Parte 2: Crear un archivo Excel (XLWorkbook, hoja, celdas, SaveAs).
///   - Parte 3: Modificar/abrir un libro existente (XLWorkbook desde stream).
///   - Parte 4: Crear tablas Excel con CreateTable().
///   - Parte 5: Aplicar estilos (negrita, color de fondo, alineación, ancho de columnas).
/// </summary>
public class ExcelReportService : IExcelReportService
{
    // =========================================================
    // REPORTE 1 – Usuarios con resumen de Tickets
    // =========================================================

    /// <inheritdoc />
    public byte[] GenerateUsersWithTicketsReport(IEnumerable<UserReportDto> data)
    {
        // PARTE 2 – Crear un XLWorkbook nuevo
        using var workbook = new XLWorkbook();

        // ---- Hoja principal ----
        var ws = workbook.Worksheets.Add("Usuarios con Tickets");

        // ---- PARTE 5 – Título de reporte (fila 1, fusionada) ----
        ws.Range("A1:I1").Merge();
        var titleCell = ws.Cell(1, 1);
        titleCell.Value = "REPORTE DE USUARIOS Y SUS TICKETS – TICKETERA DB";
        titleCell.Style.Font.Bold = true;
        titleCell.Style.Font.FontSize = 14;
        titleCell.Style.Font.FontColor = XLColor.White;
        titleCell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1F4E79");
        titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        titleCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        ws.Row(1).Height = 24;

        // Fila de fecha de generación
        ws.Range("A2:I2").Merge();
        var dateCell = ws.Cell(2, 1);
        dateCell.Value = $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}";
        dateCell.Style.Font.Italic = true;
        dateCell.Style.Font.FontColor = XLColor.Gray;
        dateCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
        ws.Row(2).Height = 15;

        // ---- PARTE 4 – Encabezados de tabla (fila 3) ----
        int headerRow = 3;
        string[] headers =
        {
            "ID Usuario", "Usuario", "Correo", "Fecha Registro",
            "Total Tickets", "Abiertos", "En Proceso", "Cerrados", "Roles"
        };

        for (int col = 1; col <= headers.Length; col++)
        {
            var cell = ws.Cell(headerRow, col);
            cell.Value = headers[col - 1];

            // PARTE 5 – Estilo encabezados
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontColor = XLColor.White;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#2E75B6");
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }
        ws.Row(headerRow).Height = 20;

        // ---- Datos reales de la BD ----
        var dataList = data.ToList();
        int row = headerRow + 1;
        foreach (var item in dataList)
        {
            ws.Cell(row, 1).Value = item.UserId.ToString();
            ws.Cell(row, 2).Value = item.Username;
            ws.Cell(row, 3).Value = item.Email ?? "Sin correo";
            ws.Cell(row, 4).Value = item.CreatedAt;
            ws.Cell(row, 4).Style.NumberFormat.Format = "dd/MM/yyyy HH:mm";
            ws.Cell(row, 5).Value = item.TotalTickets;
            ws.Cell(row, 6).Value = item.TicketsAbiertos;
            ws.Cell(row, 7).Value = item.TicketsEnProceso;
            ws.Cell(row, 8).Value = item.TicketsCerrados;
            ws.Cell(row, 9).Value = item.Roles;

            // Colores alternos por fila
            if (row % 2 == 0)
            {
                ws.Range(row, 1, row, 9).Style.Fill.BackgroundColor = XLColor.FromHtml("#DEEAF1");
            }

            // Borde delgado en cada celda de datos
            ws.Range(row, 1, row, 9).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range(row, 1, row, 9).Style.Border.InsideBorder = XLBorderStyleValues.Hair;

            row++;
        }

        // PARTE 4 – Crear tabla Excel a partir del rango de datos
        if (dataList.Count > 0)
        {
            var tableRange = ws.Range(headerRow, 1, row - 1, 9);
            var table = tableRange.CreateTable("TblUsuariosTickets");
            table.Theme = XLTableTheme.TableStyleMedium2;
        }

        // PARTE 5 – Ajustar ancho de columnas automáticamente
        ws.Columns().AdjustToContents();
        // Asegurar ancho mínimo razonable para columnas de ID (UUID largas)
        ws.Column(1).Width = 38;
        ws.Column(9).Width = 30;

        // Freeze pane en fila 4 (encabezados visibles al hacer scroll)
        ws.SheetView.FreezeRows(3);

        // PARTE 2 – Serializar a bytes (equivalente a SaveAs, usando MemoryStream)
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    // =========================================================
    // REPORTE 2 – Tickets con sus Respuestas (2 hojas)
    // =========================================================

    /// <inheritdoc />
    public byte[] GenerateTicketsWithResponsesReport(
        IEnumerable<TicketReportDto> tickets,
        IEnumerable<TicketResponseReportDto> responses)
    {
        // PARTE 2 – Crear un XLWorkbook nuevo con dos hojas
        using var workbook = new XLWorkbook();

        // ---- HOJA 1: Resumen de Tickets ----
        BuildTicketsSummarySheet(workbook, tickets.ToList());

        // ---- HOJA 2: Detalle de Respuestas ----
        // PARTE 3 – Trabajar sobre el mismo workbook (equivalente a abrir y modificar)
        BuildResponsesDetailSheet(workbook, responses.ToList());

        // PARTE 2 – Serializar a bytes
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    // ---------------------------------------------------------
    // Hoja 1: Resumen de Tickets
    // ---------------------------------------------------------
    private static void BuildTicketsSummarySheet(XLWorkbook workbook, List<TicketReportDto> tickets)
    {
        var ws = workbook.Worksheets.Add("Resumen de Tickets");

        // PARTE 5 – Título
        ws.Range("A1:H1").Merge();
        var title = ws.Cell(1, 1);
        title.Value = "REPORTE DE TICKETS – TICKETERA DB";
        title.Style.Font.Bold = true;
        title.Style.Font.FontSize = 14;
        title.Style.Font.FontColor = XLColor.White;
        title.Style.Fill.BackgroundColor = XLColor.FromHtml("#375623");
        title.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        ws.Row(1).Height = 24;

        ws.Range("A2:H2").Merge();
        ws.Cell(2, 1).Value = $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}";
        ws.Cell(2, 1).Style.Font.Italic = true;
        ws.Cell(2, 1).Style.Font.FontColor = XLColor.Gray;
        ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

        // PARTE 4 – Encabezados
        int headerRow = 3;
        string[] headers =
        {
            "ID Ticket", "Usuario Creador", "Correo", "Título",
            "Descripción", "Estado", "Fecha Creación", "Fecha Cierre",
            "Total Respuestas"
        };

        // Reajuste: 9 columnas
        ws.Range("A1:I1").Merge();
        ws.Range("A2:I2").Merge();

        string[] headers2 =
        {
            "ID Ticket", "Usuario Creador", "Correo", "Título",
            "Descripción", "Estado", "Fecha Creación", "Fecha Cierre",
            "Total Respuestas"
        };

        for (int col = 1; col <= headers2.Length; col++)
        {
            var cell = ws.Cell(headerRow, col);
            cell.Value = headers2[col - 1];

            // PARTE 5 – Estilo encabezados
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontColor = XLColor.White;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#538135");
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }
        ws.Row(headerRow).Height = 20;

        // Datos
        int row = headerRow + 1;
        foreach (var t in tickets)
        {
            ws.Cell(row, 1).Value = t.TicketId.ToString();
            ws.Cell(row, 2).Value = t.UsuarioCreador;
            ws.Cell(row, 3).Value = t.EmailCreador ?? "Sin correo";
            ws.Cell(row, 4).Value = t.Titulo;
            ws.Cell(row, 5).Value = t.Descripcion ?? string.Empty;
            ws.Cell(row, 6).Value = t.Estado;
            ws.Cell(row, 7).Value = t.FechaCreacion;
            ws.Cell(row, 7).Style.NumberFormat.Format = "dd/MM/yyyy HH:mm";
            ws.Cell(row, 8).Value = t.FechaCierre.HasValue ? t.FechaCierre.Value.ToString("dd/MM/yyyy HH:mm") : "Abierto";
            ws.Cell(row, 9).Value = t.TotalRespuestas;

            // Color según estado
            var estadoColor = t.Estado switch
            {
                "abierto"    => XLColor.FromHtml("#D9EAD3"),
                "en_proceso" => XLColor.FromHtml("#FFF2CC"),
                "cerrado"    => XLColor.FromHtml("#FCE4D6"),
                _            => XLColor.White
            };
            ws.Range(row, 1, row, 9).Style.Fill.BackgroundColor = estadoColor;
            ws.Range(row, 1, row, 9).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range(row, 1, row, 9).Style.Border.InsideBorder = XLBorderStyleValues.Hair;

            row++;
        }

        // PARTE 4 – Crear tabla Excel
        if (tickets.Count > 0)
        {
            var tableRange = ws.Range(headerRow, 1, row - 1, 9);
            var table = tableRange.CreateTable("TblTickets");
            table.Theme = XLTableTheme.TableStyleMedium7;
        }

        // PARTE 5 – Ajustar ancho de columnas
        ws.Columns().AdjustToContents();
        ws.Column(1).Width = 38;
        ws.Column(5).Width = 40;
        ws.SheetView.FreezeRows(3);
    }

    // ---------------------------------------------------------
    // Hoja 2: Detalle de Respuestas (PARTE 3 – modificar workbook)
    // ---------------------------------------------------------
    private static void BuildResponsesDetailSheet(XLWorkbook workbook, List<TicketResponseReportDto> responses)
    {
        // PARTE 3 – Agregar una segunda hoja al mismo workbook existente
        var ws = workbook.Worksheets.Add("Detalle de Respuestas");

        // PARTE 5 – Título
        ws.Range("A1:F1").Merge();
        var title = ws.Cell(1, 1);
        title.Value = "DETALLE DE RESPUESTAS POR TICKET – TICKETERA DB";
        title.Style.Font.Bold = true;
        title.Style.Font.FontSize = 14;
        title.Style.Font.FontColor = XLColor.White;
        title.Style.Fill.BackgroundColor = XLColor.FromHtml("#4A235A");
        title.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        ws.Row(1).Height = 24;

        ws.Range("A2:F2").Merge();
        ws.Cell(2, 1).Value = $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}";
        ws.Cell(2, 1).Style.Font.Italic = true;
        ws.Cell(2, 1).Style.Font.FontColor = XLColor.Gray;
        ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

        // PARTE 4 – Encabezados
        int headerRow = 3;
        string[] headers =
        {
            "ID Respuesta", "ID Ticket", "Título del Ticket",
            "Respondedor", "Mensaje", "Fecha Respuesta"
        };

        for (int col = 1; col <= headers.Length; col++)
        {
            var cell = ws.Cell(headerRow, col);
            cell.Value = headers[col - 1];

            // PARTE 5 – Estilo encabezados
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontColor = XLColor.White;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#7030A0");
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }
        ws.Row(headerRow).Height = 20;

        // Datos
        int row = headerRow + 1;
        foreach (var r in responses)
        {
            ws.Cell(row, 1).Value = r.ResponseId.ToString();
            ws.Cell(row, 2).Value = r.TicketId.ToString();
            ws.Cell(row, 3).Value = r.TituloTicket;
            ws.Cell(row, 4).Value = r.Respondedor;
            ws.Cell(row, 5).Value = r.Mensaje;
            ws.Cell(row, 6).Value = r.FechaRespuesta;
            ws.Cell(row, 6).Style.NumberFormat.Format = "dd/MM/yyyy HH:mm";

            if (row % 2 == 0)
            {
                ws.Range(row, 1, row, 6).Style.Fill.BackgroundColor = XLColor.FromHtml("#EDE7F6");
            }
            ws.Range(row, 1, row, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range(row, 1, row, 6).Style.Border.InsideBorder = XLBorderStyleValues.Hair;

            row++;
        }

        // PARTE 4 – Crear tabla Excel
        if (responses.Count > 0)
        {
            var tableRange = ws.Range(headerRow, 1, row - 1, 6);
            var table = tableRange.CreateTable("TblRespuestas");
            table.Theme = XLTableTheme.TableStyleMedium12;
        }

        // PARTE 5 – Ajustar ancho de columnas
        ws.Columns().AdjustToContents();
        ws.Column(1).Width = 38;
        ws.Column(2).Width = 38;
        ws.Column(5).Width = 50;
        ws.SheetView.FreezeRows(3);
    }

    // =========================================================================
    // IMPLEMENTACIÓN DE LAS 5 PARTES DEL LABORATORIO SEGÚN LA GUÍA DEL DOCENTE
    // =========================================================================

    /// <summary>
    /// PARTE 2: Creación de un Archivo Excel con ClosedXML
    /// Paso 1: Crear un archivo Excel vacío / Primer Ejemplo (FirstExample)
    /// </summary>
    public string FirstExample(string outputPath = "")
    {
        if (string.IsNullOrWhiteSpace(outputPath))
        {
            outputPath = Path.Combine(Directory.GetCurrentDirectory(), "FirstExample.xlsx");
        }

        // 1. Crear el libro de trabajo
        using (var workbook = new XLWorkbook())
        {
            // 2. Agregar una hoja de trabajo
            var worksheet = workbook.Worksheets.Add("Sample Sheet");

            // 3. Escribir datos básicos en celdas
            worksheet.Cell("A1").Value = "Hello World!";
            worksheet.Cell("A2").Value = 123;
            worksheet.Cell("A3").Value = DateTime.Now;

            // 4. Guardar el archivo en disco
            workbook.SaveAs(outputPath);
        }

        return outputPath;
    }

    /// <summary>
    /// PARTE 3: Modificar un archivo Excel existente
    /// Abre un XLWorkbook desde ruta, modifica una celda y guarda con Save().
    /// </summary>
    public void ModifyExcelExample(string filePath)
    {
        if (!File.Exists(filePath))
        {
            // Si no existe, lo crea primero con FirstExample
            FirstExample(filePath);
        }

        using (var workbook = new XLWorkbook(filePath))
        {
            var worksheet = workbook.Worksheet(1); // Primera hoja
            worksheet.Cell("B1").Value = "Modificado desde C#";
            worksheet.Cell("B2").Value = DateTime.Now;
            workbook.Save();
        }
    }

    /// <summary>
    /// PARTE 4: Crear tablas en Excel con CreateTable
    /// </summary>
    public string CreateTableExample(string outputPath = "")
    {
        if (string.IsNullOrWhiteSpace(outputPath))
        {
            outputPath = Path.Combine(Directory.GetCurrentDirectory(), "TableExample.xlsx");
        }

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Tabla de Datos");

            // Encabezados
            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Nombre";
            worksheet.Cell(1, 3).Value = "Edad";

            // Registros
            worksheet.Cell(2, 1).Value = 1;
            worksheet.Cell(2, 2).Value = "Juan Perez";
            worksheet.Cell(2, 3).Value = 28;

            worksheet.Cell(3, 1).Value = 2;
            worksheet.Cell(3, 2).Value = "Maria Gomez";
            worksheet.Cell(3, 3).Value = 34;

            // Seleccionar rango y crear tabla
            var range = worksheet.Range(1, 1, 3, 3);
            range.CreateTable();

            workbook.SaveAs(outputPath);
        }

        return outputPath;
    }

    /// <summary>
    /// PARTE 5: Aplicar estilos a un archivo Excel
    /// </summary>
    public string ApplyStylesExample(string outputPath = "")
    {
        if (string.IsNullOrWhiteSpace(outputPath))
        {
            outputPath = Path.Combine(Directory.GetCurrentDirectory(), "StyledExample.xlsx");
        }

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Estilos");

            // Encabezados
            worksheet.Cell(1, 1).Value = "ID";
            worksheet.Cell(1, 2).Value = "Nombre";
            worksheet.Cell(1, 3).Value = "Edad";

            // Aplicar negrita a la primera fila
            worksheet.Row(1).Style.Font.Bold = true;

            // Color de fondo a los encabezados
            worksheet.Range(1, 1, 1, 3).Style.Fill.BackgroundColor = XLColor.BabyBlue;

            // Alinear al centro
            worksheet.Range(1, 1, 1, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Registros
            worksheet.Cell(2, 1).Value = 1;
            worksheet.Cell(2, 2).Value = "Carlos Ruiz";
            worksheet.Cell(2, 3).Value = 30;

            // Ajustar ancho de columnas automáticamente
            worksheet.Columns().AdjustToContents();

            workbook.SaveAs(outputPath);
        }

        return outputPath;
    }
}

