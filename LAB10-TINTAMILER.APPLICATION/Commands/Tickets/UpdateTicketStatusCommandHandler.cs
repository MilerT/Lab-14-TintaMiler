using LAB10_TINTAMILER.DOMAIN.Interfaces;
using MediatR;

namespace LAB10_TINTAMILER.APPLICATION.Commands.Tickets;

/// <summary>
/// Handler del comando UpdateTicketStatusCommand.
/// Gestiona la transición de estado ('abierto', 'en_proceso', 'cerrado') y actualiza ClosedAt.
/// </summary>
internal sealed class UpdateTicketStatusCommandHandler : IRequestHandler<UpdateTicketStatusCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTicketStatusCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateTicketStatusCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.Tickets.GetByIdAsync(request.TicketId);
        if (ticket == null)
        {
            return false;
        }

        ticket.Status = request.Status.ToLower();
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
