using BusRouteControl.Domain.Entities;
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
        public IActionResult Index()
        {
            var BusRouteSchedule = _context.BusRoutes
        .Include(r => r.Schedules)
        .Select(r => new BusRouteScheduleViewModel
        {
            BusRouteId = r.Id,
            RouteName = r.Name,
            RouteOrigin = r.Origin,
            RouteDestination = r.Destination,
            Schedules = r.Schedules.Select(s => new ScheduleViewModel
            {
                DepartureTime = s.DepartureTime,
                ArrivalTime = s.ArrivalTime
            }).ToList()
        }).ToList();

            return View(BusRouteSchedule);
        }
        public IActionResult Create()
        {
            var viewModel = new BusRouteScheduleViewModel
            {
                RouteName = "",
                RouteOrigin = "",
                RouteDestination = "",
                Schedules = new List<ScheduleViewModel> { new ScheduleViewModel() }
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(BusRouteScheduleViewModel obj)
        {
            BusRoute busroute = new BusRoute
            {
                Name = obj.RouteName,
                Origin = obj.RouteOrigin,
                Destination = obj.RouteDestination
            };
            busroute.Schedules = obj.Schedules.Select(s => new Schedule
            {
                DepartureTime = s.DepartureTime,
                ArrivalTime = s.ArrivalTime,
                Route = busroute
            }).ToList();

            if (ModelState.IsValid)
            {
                _context.BusRoutes.Add(busroute);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);

        }
        [HttpGet]
        public IActionResult Update(int routeid)
        {
            BusRoute? busRoute = _context.BusRoutes.Include(r => r.Schedules).FirstOrDefault(r => r.Id == routeid);
            if (busRoute == null)
            {
                return NotFound();
            }
            var viewModel = new BusRouteScheduleViewModel
            {
                BusRouteId = busRoute.Id,
                RouteName = busRoute.Name,
                RouteOrigin = busRoute.Origin,
                RouteDestination = busRoute.Destination,
                Schedules = busRoute.Schedules.Select(s => new ScheduleViewModel
                {
                    Id = s.Id,
                    DepartureTime = s.DepartureTime,
                    ArrivalTime = s.ArrivalTime
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Update(BusRouteScheduleViewModel obj)
        {
            BusRoute busroute = new BusRoute
            {
                Name = obj.RouteName,
                Origin = obj.RouteOrigin,
                Destination = obj.RouteDestination
            };
            busroute.Schedules = obj.Schedules.Select(s => new Schedule
            {
                Id = s.Id,
                DepartureTime = s.DepartureTime,
                ArrivalTime = s.ArrivalTime,
                Route = busroute
            }).ToList();

            if (ModelState.IsValid)
            {
                _context.BusRoutes.Update(busroute);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);

        }

        public IActionResult Delete(int routeid)
        {
            BusRoute? busRoute = _context.BusRoutes.Include(r => r.Schedules).FirstOrDefault(r => r.Id == routeid);
            if (busRoute == null)
            {
                return NotFound();
            }
            var viewModel = new BusRouteScheduleViewModel
            {
                BusRouteId = busRoute.Id,
                RouteName = busRoute.Name,
                RouteOrigin = busRoute.Origin,
                RouteDestination = busRoute.Destination,
                Schedules = busRoute.Schedules.Select(s => new ScheduleViewModel
                {
                    Id = s.Id,
                    DepartureTime = s.DepartureTime,
                    ArrivalTime = s.ArrivalTime
                }).ToList()
            };

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Delete(BusRouteScheduleViewModel viewmodel)
        {
            BusRoute? busRoute = _context.BusRoutes.Include(r => r.Schedules).FirstOrDefault(r => r.Id == viewmodel.BusRouteId);
            if (busRoute == null)
            {
                return NotFound();
            }
            _context.BusRoutes.Remove(busRoute);
            _context.SaveChanges();
            TempData["success"] = "The Route has been deleted succesfully.";
            return RedirectToAction("Index");
        }

    }

}
