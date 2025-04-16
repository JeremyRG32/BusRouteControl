using BusRouteControl.Application.Contracts;
using BusRouteControl.Infrastructure.Contracts;
using BusRouteControl.Infrastructure.Models;


namespace BusRouteControl.Application.Services
{
    public class BusRouteService : IBusRouteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BusRouteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<BusRouteModel>> GetAllAsync()
        {
            return await _unitOfWork.BusRoutes.GetAll();
        }

        public async Task<BusRouteModel?> GetByIdAsync(int id)
        {
            return await _unitOfWork.BusRoutes.GetBusRouteById(id);
        }

        public async Task<BusRouteModel> CreateAsync(BusRouteModel model)
        {
            var result = await _unitOfWork.BusRoutes.Create(model);
            await _unitOfWork.SaveAsync();
            return result;
        }

        public async Task<bool> UpdateAsync(int id, BusRouteModel model)
        {
            var updated = await _unitOfWork.BusRoutes.UpdateBusRoute(id, model);
            if (updated)
                await _unitOfWork.SaveAsync();
            return updated;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deleted = await _unitOfWork.BusRoutes.DeleteBusRoute(id);
            if (deleted)
                await _unitOfWork.SaveAsync();
            return deleted;
        }
    }

}
