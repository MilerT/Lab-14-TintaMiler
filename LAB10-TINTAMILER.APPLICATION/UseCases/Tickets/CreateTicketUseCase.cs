using LAB10_TINTAMILER.APPLICATION.Common.Mappings;
using LAB10_TINTAMILER.APPLICATION.DTOs.Tickets;
using LAB10_TINTAMILER.DOMAIN.Entities;
using LAB10_TINTAMILER.DOMAIN.Interfaces;

namespace LAB10_TINTAMILER.APPLICATION.UseCases.Tickets;

public interface ICreateTicketUseCase
{
    Task<TicketDto> ExecuteAsync(Guid userId, CreateTicketDto dto);
}

public class CreateTicketUseCase : ICreateTicketUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTicketUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TicketDto> ExecuteAsync(Guid userId, CreateTicketDto dto)
    {
        var ticket = new Ticket
        {
            TicketId = Guid.NewGuid(),
            UserId = userId,
            Title = dto.Title,
            Description = dto.Description,
            Status = "abierto",
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Tickets.AddAsync(ticket);
        await _unitOfWork.CommitAsync();

        // Recuperar con datos del creador y relaciones
        var created = await _unitOfWork.Tickets.GetByIdWithDetailsAsync(ticket.TicketId);
        return (created ?? ticket).ToDto();
    }
}
