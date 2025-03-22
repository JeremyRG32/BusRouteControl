using BusRouteControl.Application.Common.Interfaces;
using BusRouteControl.Domain.Entities;
using System.Linq.Expressions;

namespace BusRouteControl.Infrastructure.Repository
{
    public class RouteRepository : IRouteRepository
    {
        public void Add(BusRoute busRoute)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusRoute> Get(Expression<Func<BusRoute, bool>>? filter, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusRoute> GetAll(Expression<Func<BusRoute, bool>>? filter = null, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public void Remove(BusRoute busRoute)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(BusRoute busRoute)
        {
            throw new NotImplementedException();
        }
    }
}
