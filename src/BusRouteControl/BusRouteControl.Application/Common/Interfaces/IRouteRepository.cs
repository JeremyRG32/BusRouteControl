using BusRouteControl.Domain.Entities;


namespace BusRouteControl.Application.Common.Interfaces
{
    public interface IRouteRepository : IRepository<BusRoute>
    {
        void Update(BusRoute busRoute);
        void Save();
    }
}
