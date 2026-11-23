using System.ComponentModel.DataAnnotations;

namespace LAB10_TINTAMILER.APPLICATION.DTOs.Tickets;

public class CreateTicketDto
{
    [Required(ErrorMessage = "El título del ticket es obligatorio")]
    [MaxLength(255, ErrorMessage = "El título no puede superar los 255 caracteres")]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }
}
