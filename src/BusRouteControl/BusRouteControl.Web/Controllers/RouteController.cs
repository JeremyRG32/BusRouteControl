using BusRouteControl.Domain.Dtos;
using BusRouteControl.Infrastructure.Models;
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
        public async Task<IActionResult> Update(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7192/BusRoute/Get/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var busRoute = JsonConvert.DeserializeObject<BusRouteScheduleViewModel>(json);
            return View(busRoute);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateConfirm(BusRouteScheduleViewModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"https://localhost:7192/BusRoute/Update/{model.Id}", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Reserve(int scheduleId, decimal price)
        {
            var userresponse = await _httpClient.GetAsync($"https://localhost:7192/api/User/GetByEmail/{HttpContext.Session.GetString("UserEmail")}");
            if (!userresponse.IsSuccessStatusCode)
                return Unauthorized();

            var json = await userresponse.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserDto>(json);
            var ticket = new TicketModel
            {
                UserId = user.Id,
                ScheduleId = scheduleId,
                Price = price,
                Status = "Reserved",
                BookingDate = DateTime.Now
            };

            var content = new StringContent(JsonConvert.SerializeObject(ticket), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7192/api/Ticket/Create", content);

            TempData["SuccessMessage"] = "Ticket created successfully!";
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));


            return RedirectToAction("Index", "Route");
        }


    }
}
