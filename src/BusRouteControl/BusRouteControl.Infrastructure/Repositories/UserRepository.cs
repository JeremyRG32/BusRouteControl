using BusRouteControl.Domain.Entities;
using BusRouteControl.Infrastructure.Context;
using BusRouteControl.Infrastructure.Core;
using BusRouteControl.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace BusRouteControl.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        private readonly BusRouteControlDbContext _context;

        public UserRepository(BusRouteControlDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersWithTicketsAsync()
        {
            return await _context.Users
                .Include(u => u.Tickets)
                .ToListAsync();
        }

        public async Task<User?> GetUserWithTicketsByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Tickets)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<int> CreateUserAsync(UserModel model)
        {
            var user = new User
            {
                Id = model.Id,
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                Role = model.Role,
                Tickets = model.Tickets.Select(t => new Ticket
                {
                    ScheduleId = t.ScheduleId,
                    Price = t.Price,
                    Status = t.Status,
                    BookingDate = t.BookingDate
                }).ToList()
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async Task<bool> UpdateUserAsync(UserModel model)
        {
            var existingUser = await _context.Users.FindAsync(model.Id);
            if (existingUser == null)
                return false;

            existingUser.Name = model.Name;
            existingUser.Email = model.Email;
            existingUser.Password = model.Password;
            existingUser.Role = model.Role;

            _context.Entry(existingUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
