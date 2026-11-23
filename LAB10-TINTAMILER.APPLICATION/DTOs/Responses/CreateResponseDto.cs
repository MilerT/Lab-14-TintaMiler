using System.ComponentModel.DataAnnotations;

namespace LAB10_TINTAMILER.APPLICATION.DTOs.Responses;

public class CreateResponseDto
{
    [Required(ErrorMessage = "El mensaje de respuesta es obligatorio")]
    public string Message { get; set; } = string.Empty;
}
