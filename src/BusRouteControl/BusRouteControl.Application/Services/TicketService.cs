using BusRouteControl.Application.Contracts;
using BusRouteControl.Domain.Entities;
using BusRouteControl.Infrastructure.Contracts;
using BusRouteControl.Infrastructure.Models;


namespace BusRouteControl.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Ticket>> GetAllTicketsAsync()
        {
            return await _unitOfWork.Tickets.GetAllTicketsAsync();
        }

        public async Task<Ticket?> GetTicketByIdAsync(int id)
        {
            return await _unitOfWork.Tickets.GetTicketByIdAsync(id);
        }

        public async Task<int> CreateTicketAsync(TicketModel model)
        {
            var ticketId = await _unitOfWork.Tickets.CreateTicketAsync(model);
            await _unitOfWork.SaveAsync();
            return ticketId;
        }

        public async Task<bool> UpdateTicketAsync(TicketModel model)
        {
            var updated = await _unitOfWork.Tickets.UpdateTicketAsync(model);
            if (!updated)
                return false;

            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteTicketAsync(int id)
        {
            var deleted = await _unitOfWork.Tickets.DeleteTicketAsync(id);
            if (!deleted)
                return false;

            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
