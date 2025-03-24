using BusRouteControl.Application.Common.Interfaces;
using BusRouteControl.Application.DTOs;
using BusRouteControl.Application.Services.BusRouteControl.Application.Services;
using BusRouteControl.Domain.Entities;

namespace BusRouteControl.Application.Services
{
    public class BusRouteService : IBusRouteService
    {
        private readonly IRouteRepository _routeRepository;
        private readonly IScheduleRepository _scheduleRepository;

        public BusRouteService(IRouteRepository routeRepository, IScheduleRepository scheduleRepository)
        {
            _routeRepository = routeRepository;
            _scheduleRepository = scheduleRepository;
        }

        public async Task<BusRouteDto> CreateBusRouteAsync(BusRouteDto busRouteDto)
        {
            var busRoute = new BusRoute
            {
                Name = busRouteDto.Name,
                Origin = busRouteDto.Origin,
                Destination = busRouteDto.Destination
            };

            foreach (var scheduleDto in busRouteDto.Schedules)
            {
                var schedule = new Schedule
                {
                    DepartureTime = scheduleDto.DepartureTime,
                    ArrivalTime = scheduleDto.ArrivalTime,
                    Route = busRoute
                };
                await _scheduleRepository.AddAsync(schedule);
            }

            await _routeRepository.AddAsync(busRoute);

            var resultDto = new BusRouteDto
            {
                Name = busRoute.Name,
                Origin = busRoute.Origin,
                Destination = busRoute.Destination,
                Schedules = busRoute.Schedules.Select(s => new ScheduleDto
                {
                    Id = s.Id,
                    DepartureTime = s.DepartureTime,
                    ArrivalTime = s.ArrivalTime
                }).ToList()
            };

            return resultDto;
        }

        public async Task<IEnumerable<BusRouteDto>> GetAllBusRoutesAsync()
        {
            var busRoutes = await _routeRepository.GetAllAsync();

            var busRouteDtos = busRoutes.Select(busRoute => new BusRouteDto
            {
                Name = busRoute.Name,
                Origin = busRoute.Origin,
                Destination = busRoute.Destination,
                Schedules = busRoute.Schedules.Select(s => new ScheduleDto
                {
                    Id = s.Id,
                    DepartureTime = s.DepartureTime,
                    ArrivalTime = s.ArrivalTime
                }).ToList()
            }).ToList();

            return busRouteDtos;
        }
    }



}

