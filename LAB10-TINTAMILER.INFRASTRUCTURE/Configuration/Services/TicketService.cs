using LAB10_TINTAMILER.APPLICATION.DTOs.Responses;
using LAB10_TINTAMILER.APPLICATION.DTOs.Tickets;
using LAB10_TINTAMILER.APPLICATION.Interfaces;
using LAB10_TINTAMILER.APPLICATION.UseCases.Responses;
using LAB10_TINTAMILER.APPLICATION.UseCases.Tickets;

namespace LAB10_TINTAMILER.APPLICATION.Services;

public class TicketService : ITicketService
{
    private readonly IGetAllTicketsUseCase _getAllTicketsUseCase;
    private readonly IGetTicketsByUserUseCase _getTicketsByUserUseCase;
    private readonly IGetTicketByIdUseCase _getTicketByIdUseCase;
    private readonly ICreateTicketUseCase _createTicketUseCase;
    private readonly IUpdateTicketStatusUseCase _updateTicketStatusUseCase;
    private readonly IAddResponseUseCase _addResponseUseCase;

    public TicketService(
        IGetAllTicketsUseCase getAllTicketsUseCase,
        IGetTicketsByUserUseCase getTicketsByUserUseCase,
        IGetTicketByIdUseCase getTicketByIdUseCase,
        ICreateTicketUseCase createTicketUseCase,
        IUpdateTicketStatusUseCase updateTicketStatusUseCase,
        IAddResponseUseCase addResponseUseCase)
    {
        _getAllTicketsUseCase = getAllTicketsUseCase;
        _getTicketsByUserUseCase = getTicketsByUserUseCase;
        _getTicketByIdUseCase = getTicketByIdUseCase;
        _createTicketUseCase = createTicketUseCase;
        _updateTicketStatusUseCase = updateTicketStatusUseCase;
        _addResponseUseCase = addResponseUseCase;
    }

    public Task<IEnumerable<TicketDto>> GetAllTicketsAsync()
    {
        return _getAllTicketsUseCase.ExecuteAsync();
    }

    public Task<IEnumerable<TicketDto>> GetTicketsByUserIdAsync(Guid userId)
    {
        return _getTicketsByUserUseCase.ExecuteAsync(userId);
    }

    public Task<TicketDto?> GetTicketByIdAsync(Guid id)
    {
        return _getTicketByIdUseCase.ExecuteAsync(id);
    }

    public Task<TicketDto> CreateTicketAsync(Guid userId, CreateTicketDto dto)
    {
        return _createTicketUseCase.ExecuteAsync(userId, dto);
    }

    public Task<bool> UpdateTicketStatusAsync(Guid ticketId, string status)
    {
        return _updateTicketStatusUseCase.ExecuteAsync(ticketId, status);
    }

    public Task<ResponseDto?> AddResponseAsync(Guid ticketId, Guid responderId, CreateResponseDto dto)
    {
        return _addResponseUseCase.ExecuteAsync(ticketId, responderId, dto);
    }
}
