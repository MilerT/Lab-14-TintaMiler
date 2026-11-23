using LAB10_TINTAMILER.APPLICATION.DTOs.Auth;
using LAB10_TINTAMILER.APPLICATION.Interfaces;
using LAB10_TINTAMILER.DOMAIN.Interfaces;

namespace LAB10_TINTAMILER.APPLICATION.UseCases.Auth;

public interface ILoginUseCase
{
    Task<AuthResponseDto> ExecuteAsync(LoginDto dto);
}

public class LoginUseCase : ILoginUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginUseCase(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponseDto> ExecuteAsync(LoginDto dto)
    {
        var user = await _unitOfWork.Users.GetByUsernameAsync(dto.Username);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Credenciales incorrectas: usuario o contraseña no válidos.");
        }

        // Verificar hash de contraseña
        bool isValidPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!isValidPassword)
        {
            throw new UnauthorizedAccessException("Credenciales incorrectas: usuario o contraseña no válidos.");
        }

        // Cargar usuario con sus roles
        var userWithRoles = await _unitOfWork.Users.GetByIdWithRolesAsync(user.UserId) ?? user;
        var roles = userWithRoles.UserRoles.Select(ur => ur.Role.RoleName).ToList();

        if (!roles.Any())
        {
            roles.Add("Cliente");
        }

        var (token, expiration) = _jwtTokenGenerator.GenerateToken(user, roles);

        return new AuthResponseDto
        {
            Token = token,
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            Roles = roles,
            Expiration = expiration
        };
    }
}
