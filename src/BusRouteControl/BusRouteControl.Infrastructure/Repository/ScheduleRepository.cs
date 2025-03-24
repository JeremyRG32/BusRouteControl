using BusRouteControl.Application.Common.Interfaces;
using BusRouteControl.Domain.Entities;
using BusRouteControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BusRouteControl.Infrastructure.Repository
{
    public class ScheduleRepository : Repository<Schedule>, IScheduleRepository
    {
        private readonly BusRouteControlDbContext _context;

        public ScheduleRepository(BusRouteControlDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddAsync(Schedule schedule)
        {
            await _context.Schedules.AddAsync(schedule);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Schedule>> GetAllByBusRouteIdAsync(int busRouteId)
        {
            return await _context.Schedules
                                 .Where(s => s.RouteId == busRouteId)
                                 .ToListAsync();
        }
    }
}
