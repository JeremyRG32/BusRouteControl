using BusRouteControl.Infrastructure.Models;

namespace BusRouteControl.Application.Contracts
{
    public interface IScheduleService
    {
        Task<ScheduleModel> CreateScheduleAsync(ScheduleModel model);
        Task<IEnumerable<ScheduleModel>> GetAllSchedulesAsync();
        Task<bool> UpdateScheduleAsync(int id, ScheduleModel model);
        Task<ScheduleModel> GetScheduleByIdAsync(int id);
        Task<bool> DeleteScheduleAsync(int id);
    }
}