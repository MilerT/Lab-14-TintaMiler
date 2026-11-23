using LAB10_TINTAMILER.APPLICATION.DTOs.Responses;
using LAB10_TINTAMILER.APPLICATION.DTOs.Tickets;
using LAB10_TINTAMILER.DOMAIN.Entities;

namespace LAB10_TINTAMILER.APPLICATION.Common.Mappings;

public static class TicketMappingExtensions
{
    public static TicketDto ToDto(this Ticket ticket)
    {
        return new TicketDto
        {
            TicketId = ticket.TicketId,
            UserId = ticket.UserId,
            Username = ticket.User?.Username ?? "Desconocido",
            Title = ticket.Title,
            Description = ticket.Description,
            Status = ticket.Status,
            CreatedAt = ticket.CreatedAt,
            ClosedAt = ticket.ClosedAt,
            Responses = ticket.Responses?
                .Select(r => r.ToDto())
                .OrderBy(r => r.CreatedAt)
                .ToList() ?? new List<ResponseDto>()
        };
    }

    public static ResponseDto ToDto(this Response response, string? username = null)
    {
        return new ResponseDto
        {
            ResponseId = response.ResponseId,
            TicketId = response.TicketId,
            ResponderId = response.ResponderId,
            ResponderUsername = username ?? response.Responder?.Username ?? "Desconocido",
            Message = response.Message,
            CreatedAt = response.CreatedAt
        };
    }
}
