using LAB10_TINTAMILER.APPLICATION.Common.Mappings;
using LAB10_TINTAMILER.APPLICATION.DTOs.Tickets;
using LAB10_TINTAMILER.DOMAIN.Interfaces;
using MediatR;

namespace LAB10_TINTAMILER.APPLICATION.Queries.Tickets;

/// <summary>
/// Handler de la consulta GetTicketByIdQuery.
/// </summary>
internal sealed class GetTicketByIdQueryHandler : IRequestHandler<GetTicketByIdQuery, TicketDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTicketByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TicketDto?> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.Tickets.GetByIdWithDetailsAsync(request.Id);
        return ticket?.ToDto();
    }
}
