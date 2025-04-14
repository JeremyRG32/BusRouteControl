using BusRouteControl.Domain.Core;
using BusRouteControl.Domain.Entities;
using BusRouteControl.Infrastructure.Context;
using BusRouteControl.Infrastructure.Core;
using BusRouteControl.Infrastructure.Interfaces;
using BusRouteControl.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace BusRouteControl.Infrastructure.Repositories
{
    public class BusRouteRepository : BaseRepository<BusRoute>, IBusRouteRepository
    {

        public BusRouteRepository(BusRouteControlDbContext context) : base(context) { }

        public async Task<IEnumerable<BusRouteModel>> GetAll()
        {
            var routes = await Context.BusRoutes
                .Include(r => r.Schedules)
                .ToListAsync();

            return routes.Select(MapToDto);
        }

        public async Task<BusRouteModel?> GetBusRouteById(int id)
        {
            var route = await Context.BusRoutes
           .Include(r => r.Schedules)
           .FirstOrDefaultAsync(r => r.Id == id);

            return route == null ? null : MapToDto(route);
        }

        public async Task<BusRouteModel> Create(BusRouteModel model)
        {
            var busRoute = new BusRoute
            {
                Name = model.Name,
                Origin = model.Origin,
                Destination = model.Destination,
                DefaultPrice = model.DefaultPrice,
            };

            foreach (var s in model.Schedules)
            {
                busRoute.Schedules.Add(new Schedule
                {
                    DepartureTime = TimeOnly.Parse(s.DepartureTime),
                    ArrivalTime = TimeOnly.Parse(s.ArrivalTime),
                    Route = busRoute
                });
            }

            await AddEntity(busRoute);
            return MapToDto(busRoute);
        }

        public async Task<bool> UpdateBusRoute(int id, BusRouteModel model)
        {
            var route = await Context.BusRoutes.Include(r => r.Schedules).FirstOrDefaultAsync(r => r.Id == id);

            if (route == null)
                return false;

            route.Name = model.Name;
            route.Origin = model.Origin;
            route.Destination = model.Destination;
            route.DefaultPrice = model.DefaultPrice;

            var scheduleIdsToRemove = route.Schedules
                .Where(r => !model.Schedules.Any(s => s.Id == r.Id))
                .ToList();

            Context.Schedules.RemoveRange(scheduleIdsToRemove);

            foreach (var schedule in model.Schedules)
            {
                var existingSchedule = route.Schedules
                    .FirstOrDefault(r => r.Id == schedule.Id);

                if (existingSchedule != null)
                {
                    existingSchedule.DepartureTime = TimeOnly.Parse(schedule.DepartureTime);
                    existingSchedule.ArrivalTime = TimeOnly.Parse(schedule.ArrivalTime);
                }
                else
                {
                    var newSchedule = new Schedule
                    {
                        DepartureTime = TimeOnly.Parse(schedule.DepartureTime),
                        ArrivalTime = TimeOnly.Parse(schedule.ArrivalTime),
                        Route = route
                    };
                    route.Schedules.Add(newSchedule);
                }
            }
            return await UpdateEntity(route);
        }
        public async Task<bool> DeleteBusRoute(int id)
        {
            var route = await GetEntityById(id);

            if (route == null)
                return false;

            Context.Schedules.RemoveRange(route.Schedules);
            Context.BusRoutes.Remove(route);
            await Context.SaveChangesAsync();

            return true;
        }
        private BusRouteModel MapToDto(BusRoute route)
        {
            return new BusRouteModel
            {
                Id = route.Id,
                Name = route.Name,
                Origin = route.Origin,
                Destination = route.Destination,
                DefaultPrice = route.DefaultPrice,
                Schedules = route.Schedules.Select(s => new ScheduleModel
                {
                    Id = s.Id,
                    DepartureTime = s.DepartureTime.ToString("HH:mm"),
                    ArrivalTime = s.ArrivalTime.ToString("HH:mm")
                }).ToList()
            };
        }
    }
}
