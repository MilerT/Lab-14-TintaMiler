namespace LAB10_TINTAMILER.APPLICATION.DTOs.Auth;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
    public DateTime Expiration { get; set; }
}
