using BusRouteControl.Domain.Entities;
using BusRouteControl.Infrastructure.Context;
using BusRouteControl.Infrastructure.Core;
using BusRouteControl.Infrastructure.Interfaces;
using BusRouteControl.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace BusRouteControl.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(BusRouteControlDbContext context) : base(context) { }

        public async Task<List<User>> GetAllUsersWithTicketsAsync()
        {
            return await Context.Users
                .Include(u => u.Tickets)
                .ToListAsync();
        }

        public async Task<User?> GetUserWithTicketsByIdAsync(int id)
        {
            return await Context.Users
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
            Context.Users.Add(user);
            await Context.SaveChangesAsync();
            return user.Id;
        }

        public async Task<bool> UpdateUserAsync(UserModel model)
        {
            var existingUser = await Context.Users.FindAsync(model.Id);
            if (existingUser == null)
                return false;

            existingUser.Name = model.Name;
            existingUser.Email = model.Email;
            existingUser.Password = model.Password;
            existingUser.Role = model.Role;

            Context.Entry(existingUser).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await Context.Users.FindAsync(id);
            if (user == null)
                return false;

            Context.Users.Remove(user);
            await Context.SaveChangesAsync();
            return true;
        }
        public async Task<User> GetByEmailAsync(string email)
        {
            return await Context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
