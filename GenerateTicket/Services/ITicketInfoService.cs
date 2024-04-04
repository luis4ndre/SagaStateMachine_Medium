using GenerateTicket.Models;

namespace GenerateTicket.Services
{
    public interface ITicketInfoService
    {
        Task<TicketInfo> AddTicketInfo(TicketInfo ticketInfo, int attempt);
        bool RemoveTicketInfo(string TicketId);
    }
}
