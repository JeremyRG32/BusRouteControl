using AutoMapper;
using BusRouteControl.Application.Common.Interfaces;
using BusRouteControl.Application.DTOs;
using BusRouteControl.Application.Interfaces;
using BusRouteControl.Domain.Entities;

namespace BusRouteControl.Application.Services
{
    public class BusRouteService : IRouteService
    {
        private readonly IRouteRepository _busRouteRepository;
        private readonly IMapper _mapper; // AutoMapper for DTO conversion

        public BusRouteService(IRouteRepository busRouteRepository, IMapper mapper)
        {
            _busRouteRepository = busRouteRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BusRouteDto>> GetAllBusRoutesAsync()
        {
            var routes = await _busRouteRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BusRouteDto>>(routes);
        }

        public async Task<BusRouteDto?> GetBusRouteByIdAsync(int id)
        {
            var route = await _busRouteRepository.GetByIdAsync(id);
            return route == null ? null : _mapper.Map<BusRouteDto>(route);
        }

        public async Task<BusRouteDto> CreateBusRouteAsync(BusRouteDto routeDto)
        {
            var entity = _mapper.Map<BusRoute>(routeDto);
            await _busRouteRepository.AddAsync(entity);
            return _mapper.Map<BusRouteDto>(entity);
        }

        public async Task<bool> UpdateBusRouteAsync(int id, BusRouteDto routeDto)
        {
            var existingRoute = await _busRouteRepository.GetByIdAsync(id);
            if (existingRoute == null) return false;

            _mapper.Map(routeDto, existingRoute);
            await _busRouteRepository.UpdateAsync(existingRoute);
            return true;
        }

        public async Task<bool> DeleteBusRouteAsync(int id)
        {
            var route = await _busRouteRepository.GetByIdAsync(id);
            if (route == null) return false;

            await _busRouteRepository.DeleteAsync(route);
            return true;
        }
    }

}
