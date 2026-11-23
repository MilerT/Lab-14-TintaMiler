using LAB10_TINTAMILER.APPLICATION.DTOs.Auth;

namespace LAB10_TINTAMILER.APPLICATION.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
}
