using LAB10_TINTAMILER.APPLICATION.DTOs.Auth;
using LAB10_TINTAMILER.APPLICATION.Interfaces;
using LAB10_TINTAMILER.DOMAIN.Interfaces;
using MediatR;

namespace LAB10_TINTAMILER.APPLICATION.Commands.Auth;

/// <summary>
/// Handler del comando LoginCommand.
/// </summary>
internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var user = await _unitOfWork.Users.GetByUsernameAsync(dto.Username);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Credenciales incorrectas: usuario o contraseña no válidos.");
        }

        bool isValidPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!isValidPassword)
        {
            throw new UnauthorizedAccessException("Credenciales incorrectas: usuario o contraseña no válidos.");
        }

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
