using BusRouteControl.Domain.Entities;

namespace BusRouteControl.Application.Common.Interfaces
{
    public interface IScheduleRepository
    {
        Task AddAsync(Schedule schedule);
        Task<IEnumerable<Schedule>> GetAllByBusRouteIdAsync(int busRouteId);
    }
}
