using BusRouteControl.Infrastructure.Data;
using BusRouteControl.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusRouteControl.Web.Controllers
{
    public class RouteController : Controller
    {
        private readonly BusRouteControlDbContext _context;

        public RouteController(BusRouteControlDbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            var BusRouteSchedule = _context.BusRoutes.Include(r => r.Schedules)
                .Select(r => new BusRouteScheduleViewModel
                {
                    BusRouteId = r.Id,
                    RouteName = r.Name,
                    RouteOrigin = r.Origin,
                    RouteDestination = r.Destination,
                    Schedules = r.Schedules.ToList()
                }).ToList();

            return View(BusRouteSchedule);
        }

    }
}
