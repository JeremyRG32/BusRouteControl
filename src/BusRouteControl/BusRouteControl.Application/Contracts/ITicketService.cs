using BusRouteControl.Domain.Entities;
using BusRouteControl.Infrastructure.Models;

namespace BusRouteControl.Application.Contracts
{
    public interface ITicketService
    {
        Task<int> CreateTicketAsync(TicketModel model);
        Task<bool> DeleteTicketAsync(int id);
        Task<List<Ticket>> GetAllTicketsAsync();
        Task<Ticket?> GetTicketByIdAsync(int id);
        Task<bool> UpdateTicketAsync(TicketModel model);
    }
}