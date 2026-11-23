using LAB10_TINTAMILER.APPLICATION.DTOs.Auth;
using MediatR;

namespace LAB10_TINTAMILER.APPLICATION.Commands.Auth;

/// <summary>
/// Comando CQRS para autenticar credenciales de usuario y generar Token JWT.
/// </summary>
public record LoginCommand(
    LoginDto Dto
) : IRequest<AuthResponseDto>;
