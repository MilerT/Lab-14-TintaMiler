using LAB10_TINTAMILER.APPLICATION.DTOs.Responses;
using LAB10_TINTAMILER.DOMAIN.Entities;
using LAB10_TINTAMILER.DOMAIN.Interfaces;

namespace LAB10_TINTAMILER.APPLICATION.UseCases.Responses;

public interface IAddResponseUseCase
{
    Task<ResponseDto?> ExecuteAsync(Guid ticketId, Guid responderId, CreateResponseDto dto);
}

public class AddResponseUseCase : IAddResponseUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public AddResponseUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseDto?> ExecuteAsync(Guid ticketId, Guid responderId, CreateResponseDto dto)
    {
        var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);
        if (ticket == null) return null;

        var user = await _unitOfWork.Users.GetByIdAsync(responderId);
        if (user == null) return null;

        var response = new Response
        {
            ResponseId = Guid.NewGuid(),
            TicketId = ticketId,
            ResponderId = responderId,
            Message = dto.Message,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Responses.AddAsync(response);

        // Si el ticket estaba abierto y recibe respuesta, se actualiza a 'en_proceso'
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
