using LAB10_TINTAMILER.APPLICATION.Common.Mappings;
using LAB10_TINTAMILER.APPLICATION.DTOs.Tickets;
using LAB10_TINTAMILER.DOMAIN.Entities;
using LAB10_TINTAMILER.DOMAIN.Interfaces;
using MediatR;

namespace LAB10_TINTAMILER.APPLICATION.Commands.Tickets;

/// <summary>
/// Handler del comando CreateTicketCommand.
/// Aplica el principio de responsabilidad única (Single Responsibility Principle)
/// persistiendo el nuevo ticket a través de Unit of Work y devolviendo el TicketDto resultante.
/// </summary>
internal sealed class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, TicketDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTicketCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TicketDto> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = new Ticket
        {
            TicketId = Guid.NewGuid(),
            UserId = request.UserId,
            Title = request.Title,
            Description = request.Description,
            Status = "abierto",
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Tickets.AddAsync(ticket);
        await _unitOfWork.CommitAsync();

        var created = await _unitOfWork.Tickets.GetByIdWithDetailsAsync(ticket.TicketId);
        return (created ?? ticket).ToDto();
    }
}
