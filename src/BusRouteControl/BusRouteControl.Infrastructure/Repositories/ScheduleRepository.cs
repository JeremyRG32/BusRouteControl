using BusRouteControl.Domain.Entities;
using BusRouteControl.Infrastructure.Context;
using BusRouteControl.Infrastructure.Core;
using BusRouteControl.Infrastructure.Interfaces;
using BusRouteControl.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace BusRouteControl.Infrastructure.Repositories
{
    public class ScheduleRepository : BaseRepository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(BusRouteControlDbContext context) : base(context) { }

        public async Task<IEnumerable<ScheduleModel>> GetAll()
        {
            var schedules = await Context.Schedules.ToListAsync();
            return schedules.Select(MapToDto);
        }
        public async Task<ScheduleModel> Create(ScheduleModel model)
        {
            var schedule = new Schedule
            {
                RouteId = model.RouteId,
                DepartureTime = TimeOnly.Parse(model.DepartureTime),
                ArrivalTime = TimeOnly.Parse(model.ArrivalTime)
            };

            await AddEntity(schedule);
            model.Id = schedule.Id;
            return model;
        }

        public async Task<bool> Update(int id, ScheduleModel model)
        {
            var schedule = await GetEntityById(id);
            if (schedule == null)
                return false;

            schedule.RouteId = model.RouteId;
            schedule.DepartureTime = TimeOnly.Parse(model.DepartureTime);
            schedule.ArrivalTime = TimeOnly.Parse(model.ArrivalTime);

            return await UpdateEntity(schedule);
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
    }
}
