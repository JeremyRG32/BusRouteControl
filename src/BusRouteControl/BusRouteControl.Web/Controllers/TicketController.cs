using BusRouteControl.API.Dtos;
using BusRouteControl.Web.Models;
using Microsoft.AspNetCore.Mvc;


namespace BusRouteControl.Web.Controllers
{
    public class TicketController : Controller
    {
        private readonly HttpClient _httpClient;

        public TicketController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            // Fetch all tickets from the API
            var ticketsResponse = await _httpClient.GetAsync("https://localhost:7192/api/Ticket/GetAll");
            if (!ticketsResponse.IsSuccessStatusCode)
            {
                return NotFound(); // Return a 404 if the tickets aren't found
            }

            var ticketDtos = await ticketsResponse.Content.ReadFromJsonAsync<List<TicketDto>>();

            var ticketViewModels = new List<TicketViewModel>();

            foreach (var ticketDto in ticketDtos)
            {
                var userResponse = await _httpClient.GetAsync($"https://localhost:7192/api/User/Get/{ticketDto.UserId}");
                var userDto = await userResponse.Content.ReadFromJsonAsync<UserDto>();

                var scheduleResponse = await _httpClient.GetAsync($"https://localhost:7192/api/Schedule/Get/{ticketDto.ScheduleId}");
                var scheduleDto = await scheduleResponse.Content.ReadFromJsonAsync<ScheduleDto>();

                var ticketViewModel = new TicketViewModel
                {
                    Id = ticketDto.Id,
                    UserName = userDto.Name,
                    DepartureTime = scheduleDto.DepartureTime,
                    ArrivalTime = scheduleDto.ArrivalTime,
                    Price = ticketDto.Price,
                    Status = ticketDto.Status,
                    BookingDate = ticketDto.BookingDate
                };

                ticketViewModels.Add(ticketViewModel);
            }

            return View(ticketViewModels);
        }
    }
}
