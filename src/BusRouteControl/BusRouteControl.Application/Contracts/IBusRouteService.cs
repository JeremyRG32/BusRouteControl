using BusRouteControl.Infrastructure.Models;

namespace BusRouteControl.Application.Contracts
{
    public interface IBusRouteService
    {
        Task<BusRouteModel> CreateAsync(BusRouteModel model);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<BusRouteModel>> GetAllAsync();
        Task<BusRouteModel?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, BusRouteModel model);
    }
}