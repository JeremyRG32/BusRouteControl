using BusRouteControl.Domain.Entities;
using System.Linq.Expressions;


namespace BusRouteControl.Application.Common.Interfaces
{
    public interface IRouteRepository
    {
        IEnumerable<BusRoute> GetAll(Expression<Func<BusRoute, bool>>? filter = null, string? includeProperties = null);
        IEnumerable<BusRoute> Get(Expression<Func<BusRoute, bool>>? filter, string? includeProperties = null);
        void Add(BusRoute busRoute);
        void Remove(BusRoute busRoute);
        void Update(BusRoute busRoute);
        void Save();
    }
}
