using LAB10_TINTAMILER.APPLICATION.Common.Mappings;
using LAB10_TINTAMILER.APPLICATION.DTOs.Tickets;
using LAB10_TINTAMILER.DOMAIN.Interfaces;

namespace LAB10_TINTAMILER.APPLICATION.UseCases.Tickets;

public interface IGetTicketsByUserUseCase
{
    Task<IEnumerable<TicketDto>> ExecuteAsync(Guid userId);
}

public class GetTicketsByUserUseCase : IGetTicketsByUserUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTicketsByUserUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TicketDto>> ExecuteAsync(Guid userId)
    {
        var tickets = await _unitOfWork.Tickets.GetByUserIdAsync(userId);
        return tickets.Select(t => t.ToDto());
    }
}
