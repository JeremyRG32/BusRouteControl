using BusRouteControl.Domain.Core;
using BusRouteControl.Infrastructure.Context;
using BusRouteControl.Infrastructure.Contracts;
using BusRouteControl.Infrastructure.Interfaces;

namespace BusRouteControl.Infrastructure.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BusRouteControlDbContext _context;
        public IBusRouteRepository BusRoutes { get; }
        public IUserRepository Users { get; private set; }
        public ITicketRepository Tickets { get; private set; }
        public IScheduleRepository Schedules { get; private set; }

        public UnitOfWork(
        BusRouteControlDbContext context,
        IUserRepository users,
        ITicketRepository tickets,
        IScheduleRepository schedules,
        IBusRouteRepository busRoutes
    )
        {
            _context = context;
            Users = users;
            Tickets = tickets;
            Schedules = schedules;
            BusRoutes = busRoutes;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<T> GetEntityById<T>(int id) where T : BaseEntity
        {
            var dbSet = _context.Set<T>();
            return await dbSet.FindAsync(id);
        }
        public async Task<bool> DeleteEntity<T>(int id) where T : BaseEntity
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
                return false;

            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
