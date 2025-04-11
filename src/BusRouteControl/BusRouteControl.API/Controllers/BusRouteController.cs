using BusRouteControl.API.Dtos;
using BusRouteControl.Infrastructure.Models;
using BusRouteControl.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BusRouteControl.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusRouteController : ControllerBase
    {
        private readonly BusRouteRepository _busRouteRepository;

        public BusRouteController(BusRouteRepository busRouteRepository)
        {
            _busRouteRepository = busRouteRepository;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _busRouteRepository.GetAll());
        }
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetBusRoute(int id)
        {
            return Ok(await _busRouteRepository.GetBusRouteById(id));
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] BusRouteDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The Model is Invalid.");
            }
            var model = new BusRouteModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Origin = dto.Origin,
                Destination = dto.Destination,
                DefaultPrice = dto.DefaultPrice,
                Schedules = dto.Schedules.Select(s => new ScheduleModel
                {
                    DepartureTime = s.DepartureTime.ToString(),
                    ArrivalTime = s.ArrivalTime.ToString()
                }).ToList()
            };
            model = await _busRouteRepository.Create(model);
            return Ok(new
            {
                message = "Bus route created successfully.",
                data = model
            });
        }
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateBusRoute(int id, [FromBody] BusRouteDto dto)
        {
            var model = new BusRouteModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Origin = dto.Origin,
                Destination = dto.Destination,
                DefaultPrice = dto.DefaultPrice,
                Schedules = dto.Schedules.Select(s => new ScheduleModel
                {
                    Id = s.Id,
                    DepartureTime = s.DepartureTime.ToString(),
                    ArrivalTime = s.ArrivalTime.ToString()
                }).ToList()
            };
            var success = await _busRouteRepository.UpdateBusRoute(id, model);
            if (!success)
                return NotFound();

            return Ok(new { message = "Bus route updated successfully." });
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteBusRoute(int id)
        {
            var success = await _busRouteRepository.DeleteBusRoute(id);
            if (!success)
                return NotFound();

            return Ok(new { message = "Bus route deleted successfully." });
        }
    }
}
