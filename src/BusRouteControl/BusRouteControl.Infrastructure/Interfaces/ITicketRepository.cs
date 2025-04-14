using BusRouteControl.Domain.Entities;
using BusRouteControl.Infrastructure.Models;

namespace BusRouteControl.Infrastructure.Interfaces
{
    public interface ITicketRepository
    {
        Task<int> CreateTicketAsync(TicketModel model);
        Task<bool> DeleteTicketAsync(int id);
        Task<List<Ticket>> GetAllTicketsAsync();
        Task<Ticket?> GetTicketByIdAsync(int id);
        Task<bool> UpdateTicketAsync(TicketModel model);
    }
}