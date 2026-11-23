using LAB10_TINTAMILER.APPLICATION.Common.Mappings;
using LAB10_TINTAMILER.APPLICATION.DTOs.Tickets;
using LAB10_TINTAMILER.DOMAIN.Interfaces;

namespace LAB10_TINTAMILER.APPLICATION.UseCases.Tickets;

public interface IGetTicketByIdUseCase
{
    Task<TicketDto?> ExecuteAsync(Guid id);
}

public class GetTicketByIdUseCase : IGetTicketByIdUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTicketByIdUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TicketDto?> ExecuteAsync(Guid id)
    {
        var ticket = await _unitOfWork.Tickets.GetByIdWithDetailsAsync(id);
        return ticket?.ToDto();
    }
}
