using System.ComponentModel.DataAnnotations;

namespace LAB10_TINTAMILER.APPLICATION.DTOs.Auth;

public class RegisterDto
{
    [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
    public string Username { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Formato de correo electrónico inválido")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = "Cliente"; // 'Admin', 'Cliente', 'Soporte'
}
