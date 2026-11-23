using LAB10_TINTAMILER.APPLICATION.DTOs.Auth;
using MediatR;

namespace LAB10_TINTAMILER.APPLICATION.Commands.Auth;

/// <summary>
/// Comando CQRS para registrar un nuevo usuario con contraseña cifrada y rol asignado.
/// </summary>
public record RegisterCommand(
    RegisterDto Dto
) : IRequest<AuthResponseDto>;
