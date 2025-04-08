using BusRouteControl.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BusRouteControl.Web.Controllers
{
    public class RouteController : Controller
    {
        private readonly HttpClient _httpClient;

        public RouteController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("https://localhost:7192/BusRoute/GetAll");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var busRoutes = JsonConvert.DeserializeObject<List<BusRouteScheduleViewModel>>(json);
                return View(busRoutes);
            }

            return View(new List<BusRouteScheduleViewModel>());
        }

        public IActionResult Create()
        {
            var busRoute = new BusRouteScheduleViewModel
            {
                Id = 0,
                Name = "",
                Destination = "",
                Origin = "",
                Schedules = new List<ScheduleViewModel> { new ScheduleViewModel() }
            };
            return View(busRoute);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BusRouteScheduleViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7192/BusRoute/Create", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7192/BusRoute/Get/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var busRoute = JsonConvert.DeserializeObject<BusRouteScheduleViewModel>(json);
            return View(busRoute);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7192/BusRoute/Delete/{id}");
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return NotFound();
        }


        //        if (ModelState.IsValid)
        //        {
        //            _context.Add(busroute);
        //            _context.SaveChanges();
        //            return RedirectToAction("Index");
        //        }

        //        return View(obj);

        //    }
        //    [HttpGet]
        //    public IActionResult Update(int routeid)
        //    {
        //        var busRoute = _context.BusRoutes
        //        .Include(r => r.Schedules)
        //        .FirstOrDefault(r => r.Id == routeid);

        //        if (busRoute == null)
        //        {
        //            return NotFound();
        //        }
        //        var viewModel = new BusRouteScheduleViewModel
        //        {
        //            BusRouteId = busRoute.Id,
        //            RouteName = busRoute.Name,
        //            RouteOrigin = busRoute.Origin,
        //            RouteDestination = busRoute.Destination,
        //            Schedules = busRoute.Schedules.Select(s => new ScheduleViewModel
        //            {
        //                Id = s.Id,
        //                DepartureTime = s.DepartureTime,
        //                ArrivalTime = s.ArrivalTime
        //            }).ToList()
        //        };

        //        return View(viewModel);
        //    }


        //    public IActionResult Update(BusRouteScheduleViewModel obj)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return View(obj);
        //        }
        //        var busRoute = _context.BusRoutes
        //        .Include(r => r.Schedules)
        //        .FirstOrDefault(r => r.Id == obj.BusRouteId);

        //        if (busRoute == null)
        //        {
        //            return NotFound();
        //        }

        //        busRoute.Name = obj.RouteName;
        //        busRoute.Origin = obj.RouteOrigin;
        //        busRoute.Destination = obj.RouteDestination;

        //        busRoute.Schedules.Clear();
        //        foreach (var s in obj.Schedules)
        //        {
        //            busRoute.Schedules.Add(new Schedule
        //            {
        //                Id = s.Id,
        //                DepartureTime = s.DepartureTime,
        //                ArrivalTime = s.ArrivalTime,
        //                Route = busRoute
        //            });
        //        }

        //        _context.SaveChanges();

        //        return RedirectToAction("Index");
        //    }



        //    [HttpPost]
        //    public IActionResult Delete(BusRouteScheduleViewModel obj)
        //    {
        //        var busRoute = _context.BusRoutes
        //        .Include(r => r.Schedules)
        //        .FirstOrDefault(r => r.Id == obj.BusRouteId);

        //        if (busRoute == null)
        //        {
        //            return NotFound();
        //        }
        //        _context.Remove(busRoute);
        //        _context.SaveChanges();
        //        TempData["success"] = "The Route has been deleted succesfully.";
        //        return RedirectToAction("Index");
        //    }
        //}

    }
}
