﻿using BusRouteControl.Domain.Entities;
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BusRouteScheduleViewModel obj)
        {
            BusRoute busroute = new BusRoute
            {
                Name = obj.RouteName,
                Origin = obj.RouteOrigin,
                Destination = obj.RouteDestination,
                Schedules = obj.Schedules.Select(s => new Schedule
                {
                    DepartureTime = s.DepartureTime,
                    ArrivalTime = s.ArrivalTime
                }).ToList()
            };
            _context.BusRoutes.Add(busroute);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
