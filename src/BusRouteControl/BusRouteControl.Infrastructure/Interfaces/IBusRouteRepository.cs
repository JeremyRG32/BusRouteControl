using BusRouteControl.Infrastructure.Models;

namespace BusRouteControl.Infrastructure.Interfaces
{
    public interface IBusRouteRepository
    {
        Task<BusRouteModel> Create(BusRouteModel model);
        Task<bool> DeleteBusRoute(int id);
        Task<IEnumerable<BusRouteModel>> GetAll();
        Task<BusRouteModel?> GetBusRouteById(int id);
        Task<bool> UpdateBusRoute(int id, BusRouteModel model);
    }
}