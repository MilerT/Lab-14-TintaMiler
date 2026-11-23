using LAB10_TINTAMILER.APPLICATION.Common.Mappings;
using LAB10_TINTAMILER.APPLICATION.DTOs.Tickets;
using LAB10_TINTAMILER.DOMAIN.Interfaces;

namespace LAB10_TINTAMILER.APPLICATION.UseCases.Tickets;

public interface IGetAllTicketsUseCase
{
    Task<IEnumerable<TicketDto>> ExecuteAsync();
}

public class GetAllTicketsUseCase : IGetAllTicketsUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllTicketsUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TicketDto>> ExecuteAsync()
    {
        var tickets = await _unitOfWork.Tickets.GetAllWithDetailsAsync();
        return tickets.Select(t => t.ToDto());
    }
}
