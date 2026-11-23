using LAB10_TINTAMILER.APPLICATION.DTOs.Auth;
using LAB10_TINTAMILER.APPLICATION.Interfaces;
using LAB10_TINTAMILER.DOMAIN.Entities;
using LAB10_TINTAMILER.DOMAIN.Interfaces;
using MediatR;

namespace LAB10_TINTAMILER.APPLICATION.Commands.Auth;

/// <summary>
/// Handler del comando RegisterCommand.
/// </summary>
internal sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterCommandHandler(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var existingUser = await _unitOfWork.Users.GetByUsernameAsync(dto.Username);
        if (existingUser != null)
        {
            throw new InvalidOperationException($"El nombre de usuario '{dto.Username}' ya se encuentra registrado.");
        }

        if (!string.IsNullOrEmpty(dto.Email))
        {
            var existingEmail = await _unitOfWork.Users.GetByEmailAsync(dto.Email);
            if (existingEmail != null)
            {
                throw new InvalidOperationException($"El correo '{dto.Email}' ya se encuentra registrado.");
            }
        }

        var roleName = string.IsNullOrWhiteSpace(dto.Role) ? "Cliente" : dto.Role;
        var roles = await _unitOfWork.Roles.FindAsync(r => r.RoleName.ToLower() == roleName.ToLower());
        var role = roles.FirstOrDefault();

        if (role == null)
        {
            role = new Role
            {
                RoleId = Guid.NewGuid(),
                RoleName = roleName
            };
            await _unitOfWork.Roles.AddAsync(role);
        }

        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            CreatedAt = DateTime.UtcNow
        };

        var userRole = new UserRole
        {
            UserId = user.UserId,
            RoleId = role.RoleId,
            AssignedAt = DateTime.UtcNow
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.UserRoles.AddAsync(userRole);
        await _unitOfWork.CommitAsync();

        var userRolesList = new List<string> { role.RoleName };
        var (token, expiration) = _jwtTokenGenerator.GenerateToken(user, userRolesList);

        return new AuthResponseDto
        {
            Token = token,
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            Roles = userRolesList,
            Expiration = expiration
        };
    }
}
