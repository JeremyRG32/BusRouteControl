using BusRouteControl.Application.Contracts;
using BusRouteControl.Domain.Entities;
using BusRouteControl.Infrastructure.Contracts;
using BusRouteControl.Infrastructure.Models;

namespace BusRouteControl.Application.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ScheduleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ScheduleModel>> GetAllSchedulesAsync()
        {
            return await _unitOfWork.Schedules.GetAll();
        }

        public async Task<ScheduleModel> CreateScheduleAsync(ScheduleModel model)
        {
            var createdSchedule = await _unitOfWork.Schedules.Create(model);
            await _unitOfWork.SaveAsync();
            return createdSchedule;
        }

        public async Task<bool> UpdateScheduleAsync(int id, ScheduleModel model)
        {
            var updated = await _unitOfWork.Schedules.Update(id, model);
            if (!updated)
                return false;

            await _unitOfWork.SaveAsync();
            return true;
        }
        public async Task<ScheduleModel> GetScheduleByIdAsync(int id)
        {
            var schedule = await _unitOfWork.GetEntityById<Schedule>(id);
            if (schedule == null)
                return null;
            return MapToDto(schedule);
        }
        private ScheduleModel MapToDto(Schedule schedule)
        {
            return new ScheduleModel
            {
                Id = schedule.Id,
                RouteId = schedule.RouteId,
                DepartureTime = schedule.DepartureTime.ToString("HH:mm"),
                ArrivalTime = schedule.ArrivalTime.ToString("HH:mm")
            };
        }
        public async Task<bool> DeleteScheduleAsync(int id)
        {
            var schedule = await _unitOfWork.GetEntityById<Schedule>(id);
            if (schedule == null)
                return false;

            return await _unitOfWork.DeleteEntity<Schedule>(id);
        }
    }
}
