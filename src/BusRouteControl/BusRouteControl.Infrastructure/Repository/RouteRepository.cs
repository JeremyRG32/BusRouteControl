using BusRouteControl.Application.Common.Interfaces;
using BusRouteControl.Domain.Entities;
using BusRouteControl.Infrastructure.Data;

namespace BusRouteControl.Infrastructure.Repository
{
    public class RouteRepository : Repository<BusRoute>, IRouteRepository
    {
        private readonly BusRouteControlDbContext _context;

        public RouteRepository(BusRouteControlDbContext context) : base(context)
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(BusRoute busRoute)
        {
            _context.Update(busRoute);
        }
    }
}
