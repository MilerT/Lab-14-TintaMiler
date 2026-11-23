using System.ComponentModel.DataAnnotations;

namespace LAB10_TINTAMILER.APPLICATION.DTOs.Tickets;

public class UpdateTicketStatusDto
{
    [Required(ErrorMessage = "El estado es obligatorio")]
    [RegularExpression("^(abierto|en_proceso|cerrado)$", ErrorMessage = "El estado debe ser: abierto, en_proceso o cerrado")]
    public string Status { get; set; } = "abierto";
}
