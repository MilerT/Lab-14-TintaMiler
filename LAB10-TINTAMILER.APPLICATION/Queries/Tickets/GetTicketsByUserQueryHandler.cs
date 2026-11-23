using LAB10_TINTAMILER.APPLICATION.Common.Mappings;
using LAB10_TINTAMILER.APPLICATION.DTOs.Tickets;
using LAB10_TINTAMILER.DOMAIN.Interfaces;
using MediatR;

namespace LAB10_TINTAMILER.APPLICATION.Queries.Tickets;

/// <summary>
/// Handler de la consulta GetTicketsByUserQuery.
/// </summary>
internal sealed class GetTicketsByUserQueryHandler : IRequestHandler<GetTicketsByUserQuery, IEnumerable<TicketDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTicketsByUserQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TicketDto>> Handle(GetTicketsByUserQuery request, CancellationToken cancellationToken)
    {
        var tickets = await _unitOfWork.Tickets.GetByUserIdAsync(request.UserId);
        return tickets.Select(t => t.ToDto());
    }
}
