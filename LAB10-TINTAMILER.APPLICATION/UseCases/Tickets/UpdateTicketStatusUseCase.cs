using LAB10_TINTAMILER.DOMAIN.Interfaces;

namespace LAB10_TINTAMILER.APPLICATION.UseCases.Tickets;

public interface IUpdateTicketStatusUseCase
{
    Task<bool> ExecuteAsync(Guid ticketId, string status);
}

public class UpdateTicketStatusUseCase : IUpdateTicketStatusUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTicketStatusUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> ExecuteAsync(Guid ticketId, string status)
    {
        var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);
        if (ticket == null) return false;

        ticket.Status = status.ToLower();
        if (ticket.Status == "cerrado")
        {
            ticket.ClosedAt = DateTime.UtcNow;
        }
        else
        {
            ticket.ClosedAt = null;
        }

        _unitOfWork.Tickets.Update(ticket);
        await _unitOfWork.CommitAsync();
        return true;
    }
}
