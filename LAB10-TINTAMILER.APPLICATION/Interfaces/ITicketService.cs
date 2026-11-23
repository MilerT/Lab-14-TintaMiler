using LAB10_TINTAMILER.APPLICATION.DTOs.Responses;
using LAB10_TINTAMILER.APPLICATION.DTOs.Tickets;

namespace LAB10_TINTAMILER.APPLICATION.Interfaces;

public interface ITicketService
{
    Task<IEnumerable<TicketDto>> GetAllTicketsAsync();
    Task<IEnumerable<TicketDto>> GetTicketsByUserIdAsync(Guid userId);
    Task<TicketDto?> GetTicketByIdAsync(Guid id);
    Task<TicketDto> CreateTicketAsync(Guid userId, CreateTicketDto dto);
    Task<bool> UpdateTicketStatusAsync(Guid ticketId, string status);
    Task<ResponseDto?> AddResponseAsync(Guid ticketId, Guid responderId, CreateResponseDto dto);
}
