using BusRouteControl.Application.DTOs;

namespace BusRouteControl.Application.Services
{
    namespace BusRouteControl.Application.Services
    {
        public interface IBusRouteService
        {
            Task<BusRouteDto> CreateBusRouteAsync(BusRouteDto busRouteDto);
            Task<IEnumerable<BusRouteDto>> GetAllBusRoutesAsync();
        }
    }

}
