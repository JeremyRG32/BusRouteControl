using BusRouteControl.Domain.Dtos;
using BusRouteControl.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace BusRouteControl.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            var successMessage = TempData["SuccessMessage"] as string;
            ViewBag.SuccessMessage = successMessage;
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string role)
        {
            var loginDto = new
            {
                Email = email,
                Password = password,
                Role = role
            };

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://localhost:7192/api/User/login");

                var response = await httpClient.PostAsJsonAsync("login", loginDto);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResultDto>();

                    HttpContext.Session.SetString("UserRole", result.Role);
                    HttpContext.Session.SetString("UserEmail", result.Email);

                    if (result.Role == "Admin")
                        return RedirectToAction("Index", "Route");
                    else if (result.Role == "Client")
                        return RedirectToAction("Index", "Route");
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid credentials.";
                }
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7192/api/User/Create", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Registration successful. You can now log in.";
                return RedirectToAction("Login");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Registration failed: {errorContent}. Please try again.";
                return RedirectToAction("Login");
            }
            return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
