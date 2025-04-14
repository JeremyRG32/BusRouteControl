using BusRouteControl.Domain.Entities;
using BusRouteControl.Infrastructure.Contracts;
using BusRouteControl.Infrastructure.Models;

namespace BusRouteControl.Infrastructure.Interfaces
{
    public interface IScheduleRepository : IBaseRepository<Schedule>
    {
        Task<ScheduleModel> Create(ScheduleModel model);
        Task<IEnumerable<ScheduleModel>> GetAll();
        Task<bool> Update(int id, ScheduleModel model);
    }
}