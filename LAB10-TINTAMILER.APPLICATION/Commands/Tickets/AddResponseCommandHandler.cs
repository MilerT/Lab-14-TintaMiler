using LAB10_TINTAMILER.APPLICATION.DTOs.Responses;
using LAB10_TINTAMILER.DOMAIN.Entities;
using LAB10_TINTAMILER.DOMAIN.Interfaces;
using MediatR;

namespace LAB10_TINTAMILER.APPLICATION.Commands.Tickets;

/// <summary>
/// Handler del comando AddResponseCommand.
/// Valida la existencia del ticket y del usuario que responde, crea la respuesta
/// y actualiza el estado a 'en_proceso' si se encontraba 'abierto'.
/// </summary>
internal sealed class AddResponseCommandHandler : IRequestHandler<AddResponseCommand, ResponseDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddResponseCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseDto?> Handle(AddResponseCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.Tickets.GetByIdAsync(request.TicketId);
        if (ticket == null)
        {
            return null;
        }

        var user = await _unitOfWork.Users.GetByIdAsync(request.ResponderId);
        if (user == null)
        {
            return null;
        }

        var response = new Response
        {
            ResponseId = Guid.NewGuid(),
            TicketId = request.TicketId,
            ResponderId = request.ResponderId,
            Message = request.Message,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Responses.AddAsync(response);

        // Si el ticket estaba abierto y recibe respuesta, pasa a en_proceso
        if (ticket.Status == "abierto")
        {
            ticket.Status = "en_proceso";
            _unitOfWork.Tickets.Update(ticket);
        }

        await _unitOfWork.CommitAsync();

        return new ResponseDto
        {
            ResponseId = response.ResponseId,
            TicketId = response.TicketId,
            ResponderId = response.ResponderId,
            ResponderUsername = user.Username,
            Message = response.Message,
            CreatedAt = response.CreatedAt
        };
    }
}
