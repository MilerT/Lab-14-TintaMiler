using LAB10_TINTAMILER.APPLICATION.DTOs.Auth;
using LAB10_TINTAMILER.APPLICATION.Interfaces;
using LAB10_TINTAMILER.APPLICATION.UseCases.Auth;

namespace LAB10_TINTAMILER.APPLICATION.Services;

public class AuthService : IAuthService
{
    private readonly IRegisterUseCase _registerUseCase;
    private readonly ILoginUseCase _loginUseCase;

    public AuthService(IRegisterUseCase registerUseCase, ILoginUseCase loginUseCase)
    {
        _registerUseCase = registerUseCase;
        _loginUseCase = loginUseCase;
    }

    public Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        return _registerUseCase.ExecuteAsync(dto);
    }

    public Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        return _loginUseCase.ExecuteAsync(dto);
    }
}
