using BusRouteControl.Domain.Entities;
using BusRouteControl.Infrastructure.Context;
using BusRouteControl.Infrastructure.Core;
using BusRouteControl.Infrastructure.Interfaces;
using BusRouteControl.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace BusRouteControl.Infrastructure.Repositories
{
    public class TicketRepository : BaseRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(BusRouteControlDbContext context) : base(context) { }
        public async Task<List<Ticket>> GetAllTicketsAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<Ticket?> GetTicketByIdAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<int> CreateTicketAsync(TicketModel model)
        {
            var ticket = new Ticket
            {
                UserId = model.UserId,
                ScheduleId = model.ScheduleId,
                Price = model.Price,
                Status = model.Status,
                BookingDate = model.BookingDate
            };
            return await AddEntity(ticket);
        }

        public async Task<bool> UpdateTicketAsync(TicketModel model)
        {
            var existingticket = await Context.Tickets.FindAsync(model.Id);
            if (existingticket == null)
                return false;

            existingticket.Id = model.Id;
            existingticket.ScheduleId = model.ScheduleId;
            existingticket.Price = model.Price;
            existingticket.Status = model.Status;
            existingticket.BookingDate = model.BookingDate;

            Context.Entry(existingticket).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTicketAsync(int id)
        {
            return await DeleteEntity(id);
        }
    }
}
