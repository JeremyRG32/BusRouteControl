using BusRouteControl.Domain.Entities;


namespace BusRouteControl.Application.Common.Interfaces
{
    public interface IRouteRepository : IRepository<BusRoute>
    {
        //IQueryable<BusRoute> GetAll(Expression<Func<BusRoute, bool>>? filter = null, string? includeProperties = null);
        //IQueryable<BusRoute> Get(Expression<Func<BusRoute, bool>>? filter, string? includeProperties = null);
        //void Add(BusRoute busRoute);
        //void Remove(BusRoute busRoute);
        void Update(BusRoute busRoute);
        void Save();
    }
}
