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
    }
}
