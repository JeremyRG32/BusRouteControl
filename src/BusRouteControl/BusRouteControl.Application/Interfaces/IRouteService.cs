using BusRouteControl.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusRouteControl.Application.Interfaces
{
    public interface IRouteService
    {
        Task<IEnumerable<BusRouteDto>> GetAllBusRoutesAsync();
        Task<BusRouteDto?> GetBusRouteByIdAsync(int id);
        Task<BusRouteDto> CreateBusRouteAsync(BusRouteDto routeDto);
        Task<bool> UpdateBusRouteAsync(int id, BusRouteDto routeDto);
        Task<bool> DeleteBusRouteAsync(int id);
    }
}
